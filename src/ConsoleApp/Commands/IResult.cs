using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.ConsoleApp.Commands
{
    public interface IResult
    {
        CommandError? Error { get; }

        string ErrorReason { get; }

        bool IsSuccess { get; }
    }
}
