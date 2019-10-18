using System;

namespace TeaBagBot.Core.Storage.Memory
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
