using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Storage.Exceptions
{
    public class MemoryStorageException : Exception
    {
        public MemoryStorageException(string message) : base(message)
        {

        }

        public MemoryStorageException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
