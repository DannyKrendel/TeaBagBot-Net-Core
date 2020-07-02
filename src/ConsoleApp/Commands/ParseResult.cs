using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.ConsoleApp.Commands
{
    public struct ParseResult : IResult
    {
        public IReadOnlyList<object> ArgValues { get; }

        public CommandError? Error { get; }

        public string ErrorReason { get; }

        public bool IsSuccess => !Error.HasValue;

        private ParseResult(IReadOnlyList<object> argValues, CommandError? error = null, string errorReason = null)
        {
            ArgValues = argValues;
            Error = error;
            ErrorReason = errorReason;
        }

        public static ParseResult FromSuccess(IReadOnlyList<object> argValues)
        {
            return new ParseResult(argValues);
        }

        public static ParseResult FromError(CommandError error, string reason)
            => new ParseResult(null, error, reason);
        public static ParseResult FromError(Exception ex)
            => FromError(CommandError.Exception, ex.Message);
        public static ParseResult FromError(IResult result)
            => new ParseResult(null, result.Error, result.ErrorReason);

        public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
    }
}
