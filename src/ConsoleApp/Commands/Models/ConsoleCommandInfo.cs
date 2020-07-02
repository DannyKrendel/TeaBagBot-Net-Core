using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeaBagBot.ConsoleApp.Commands.Models
{
    public class ConsoleCommandInfo
    {
        public string Name { get; }
        public string Summary { get; }
        public bool IgnoreExtraArgs { get; }
        public IReadOnlyList<string> Aliases { get; }
        public IReadOnlyList<ConsoleParameterInfo> Parameters { get; }
        public ConsoleCommandParser Parser { get; }

        private readonly Func<IConsoleCommandContext, object[], IServiceProvider, ConsoleCommandInfo, Task> _action;

        public ConsoleCommandInfo(string name, string summary, string[] aliases, ConsoleParameterInfo[] parameters,
            ConsoleCommandParser parser, bool ignoreExtraArgs, Func<IConsoleCommandContext, object[], IServiceProvider, ConsoleCommandInfo, Task> action)
        {
            Name = name;
            Summary = summary;
            Aliases = aliases;
            Parameters = parameters;
            Parser = parser;
            IgnoreExtraArgs = ignoreExtraArgs;
            _action = action;
        }

        public async Task<ParseResult> ParseAsync(IConsoleCommandContext context, string input)
        {
            if (input == null)
                throw new ArgumentNullException($"Error in {this}. {nameof(input)} was null.");

            return await Parser.ParseArgsAsync(this, context, input);
        }

        public async Task ExecuteAsync(IConsoleCommandContext context, object[] args, IServiceProvider services)
        {
            await _action(context, args, services, this);
        }

        public override string ToString() => Name;
    }
}
