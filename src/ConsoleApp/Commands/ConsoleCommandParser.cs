using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeaBagBot.ConsoleApp.Commands.Models;
using TeaBagBot.ConsoleApp.Helpers;

namespace TeaBagBot.ConsoleApp.Commands
{
    public class ConsoleCommandParser
    {
        private enum ParserPart
        {
            None,
            Parameter,
            QuotedParameter
        }

        public async Task<ParseResult> ParseArgsAsync(ConsoleCommandInfo command, IConsoleCommandContext context, string input)
        {
            var aliasMap = QuotationAliasUtils.DefaultAliasMap;
            ConsoleParameterInfo curParam = null;
            var argBuilder = new StringBuilder(input.Length);
            int startPos = 0;
            int endPos = input.Length;
            var curPart = ParserPart.None;
            int lastArgEndPos = -1;
            var argList = new List<object>();
            bool isEscaping = false;
            char c, matchQuote = '\0';

            bool IsOpenQuote(char ch)
            {
                // return if the key is contained in the dictionary if it is populated
                if (aliasMap.Count != 0)
                    return aliasMap.ContainsKey(ch);
                // or otherwise if it is the default double quote
                return ch == '\"';
            }

            char GetMatch(char ch)
            {
                // get the corresponding value for the key, if it exists
                // and if the dictionary is populated
                if (aliasMap.Count != 0 && aliasMap.TryGetValue(c, out var value))
                    return value;
                // or get the default pair of the default double quote
                return '\"';
            }

            for (int curPos = startPos; curPos <= endPos; curPos++)
            {
                if (curPos < endPos)
                    c = input[curPos];
                else
                    c = '\0';

                //If this character is escaped, skip it
                if (isEscaping)
                {
                    if (curPos != endPos)
                    {
                        // if this character matches the quotation mark of the end of the string
                        // means that it should be escaped
                        // but if is not, then there is no reason to escape it then
                        if (c != matchQuote)
                        {
                            // if no reason to escape the next character, then re-add \ to the arg
                            argBuilder.Append('\\');
                        }

                        argBuilder.Append(c);
                        isEscaping = false;
                        continue;
                    }
                }

                //Are we escaping the next character?
                if (c == '\\' && (curParam == null || !curParam.IsRemainder))
                {
                    isEscaping = true;
                    continue;
                }

                //If we're processing an remainder parameter, ignore all other logic
                if (curParam != null && curParam.IsRemainder && curPos != endPos)
                {
                    argBuilder.Append(c);
                    continue;
                }

                //If we're not currently processing one, are we starting the next argument yet?
                if (curPart == ParserPart.None)
                {
                    if (char.IsWhiteSpace(c) || curPos == endPos)
                        continue; //Skip whitespace between arguments
                    else if (curPos == lastArgEndPos)
                        return ParseResult.FromError(CommandError.ParseFailed, "There must be at least one character of whitespace between arguments.");
                    else
                    {
                        if (curParam == null)
                            curParam = command.Parameters.Count > argList.Count ? command.Parameters[argList.Count] : null;

                        if (curParam != null && curParam.IsRemainder)
                        {
                            argBuilder.Append(c);
                            continue;
                        }

                        if (IsOpenQuote(c))
                        {
                            curPart = ParserPart.QuotedParameter;
                            matchQuote = GetMatch(c);
                            continue;
                        }
                        curPart = ParserPart.Parameter;
                    }
                }

                //Has this parameter ended yet?
                string argString = null;
                if (curPart == ParserPart.Parameter)
                {
                    if (curPos == endPos || char.IsWhiteSpace(c))
                    {
                        argString = argBuilder.ToString();
                        lastArgEndPos = curPos;
                    }
                    else
                        argBuilder.Append(c);
                }
                else if (curPart == ParserPart.QuotedParameter)
                {
                    if (c == matchQuote)
                    {
                        argString = argBuilder.ToString(); //Remove quotes
                        lastArgEndPos = curPos + 1;
                    }
                    else
                        argBuilder.Append(c);
                }

                if (argString != null)
                {
                    if (curParam == null)
                    {
                        if (command.IgnoreExtraArgs)
                            break;
                        else
                            return ParseResult.FromError(CommandError.BadArgCount, "The input text has too many parameters.");
                    }

                    var result = await curParam.ParseAsync(context, argString).ConfigureAwait(false);
                    if (result == null)
                        return ParseResult.FromError(CommandError.ParseFailed, $"Failed to parse {curParam.Name}.");

                    argList.Add(result);

                    curParam = null;
                    curPart = ParserPart.None;

                    argBuilder.Clear();
                }
            }

            if (curParam != null && curParam.IsRemainder)
            {
                var res = await curParam.ParseAsync(context, argBuilder.ToString()).ConfigureAwait(false);
                if (res == null)
                    return ParseResult.FromError(CommandError.ParseFailed, $"Failed to parse {curParam.Name}.");
                argList.Add(res);
            }

            if (isEscaping)
                return ParseResult.FromError(CommandError.ParseFailed, "Input text may not end on an incomplete escape.");
            if (curPart == ParserPart.QuotedParameter)
                return ParseResult.FromError(CommandError.ParseFailed, "A quoted parameter is incomplete.");

            //Add missing optionals
            for (int i = argList.Count; i < command.Parameters.Count; i++)
            {
                var param = command.Parameters[i];

                if (!param.IsOptional)
                    return ParseResult.FromError(CommandError.BadArgCount, "The input text has too few parameters.");

                argList.Add(param.DefaultValue);
            }

            return ParseResult.FromSuccess(argList);
        }
    }
}
