using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.ConsoleApp.Commands
{
    public enum CommandError
    {
        /// <summary>
        /// Thrown when the command is unknown.
        /// </summary>
        UnknownCommand = 1,

        /// <summary>
        /// Thrown when the command fails to be parsed.
        /// </summary>
        ParseFailed,

        /// <summary>
        /// Thrown when the input text has too few or too many arguments.
        /// </summary>
        BadArgCount,

        /// <summary>
        /// Thrown when the object cannot be found by TypeReader/>.
        /// </summary>
        ObjectNotFound,

        /// <summary>
        /// Thrown when more than one object is matched by TypeReader/>.
        /// </summary>
        MultipleMatches,

        /// <summary>
        /// Thrown when an exception occurs mid-command execution.
        /// </summary>
        Exception,

        /// <summary>
        /// Thrown when the command is not successfully executed on runtime.
        /// </summary>
        Unsuccessful
    }
}
