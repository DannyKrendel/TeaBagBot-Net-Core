using System;

namespace TeaBagBot.Discord
{
    public class ConnectionException : Exception
    {
        public ConnectionException(string message) : base(message)
        {

        }

        public ConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
