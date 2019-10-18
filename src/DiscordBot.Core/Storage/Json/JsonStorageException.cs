using System;

namespace TeaBagBot.Core.Storage.Json
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
