using System;

namespace TeaBagBot.Core
{
    public class TokenException : Exception
    {
        public TokenException(string message) : base(message)
        {

        }

        public TokenException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
