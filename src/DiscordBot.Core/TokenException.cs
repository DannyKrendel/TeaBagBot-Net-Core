using System;

namespace DiscordBot
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
