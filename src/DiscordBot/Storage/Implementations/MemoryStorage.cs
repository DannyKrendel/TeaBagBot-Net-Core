using DiscordBot.Storage.Exceptions;
using DiscordBot.Storage.Interfaces;
using System;
using System.Collections.Generic;

namespace DiscordBot.Storage.Implementations
{
    public class MemoryStorage : IDataStorage
    {
        private readonly Dictionary<string, object> dictionary;

        public MemoryStorage()
        {
            dictionary = new Dictionary<string, object>();
        }

        public void StoreObject(object obj, string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key), $"Key was null.");
            if (key == "")
                throw new ArgumentException($"Key was empty.", nameof(key));

            try
            {
                if (dictionary.ContainsKey(key)) // if dictionary already contains key
                    dictionary[key] = obj; // replace old object with new object
                else
                    dictionary.Add(key, obj); // else add object
            }
            catch (Exception ex)
            {
                throw new MemoryStorageException($"Couldn't store object '{nameof(obj)}' with key '{key}'.", ex);
            }
        }

        public T RestoreObject<T>(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key), $"Key was null.");
            if (key == "")
                throw new ArgumentException($"Key was empty.", nameof(key));
            if (!dictionary.ContainsKey(key))
                throw new ArgumentException($"Object with key '{key}' was not found in storage.", nameof(key));

            try
            {
                return (T)dictionary[key];
            }
            catch (Exception ex)
            {
                throw new MemoryStorageException($"Couldn't restore object of type '{typeof(T).Name}' with key '{key}'.", ex);
            }
        }
    }
}
