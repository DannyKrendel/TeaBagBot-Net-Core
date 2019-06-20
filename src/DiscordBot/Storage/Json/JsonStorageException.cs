using System;

namespace DiscordBot.Storage.Json
{
    public class JsonStorageException : Exception
    {
        public JsonStorageException(string message) : base(message)
        {

        }

        public JsonStorageException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
