using System;
using System.Collections.Generic;

namespace DiscordBot.Storage.Memory
{
    public class MemoryStorage : IDataStorage
    {
        private readonly Dictionary<string, object> _dictionary;

        public MemoryStorage()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public void StoreObject(object obj, string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key), $"Key was null.");
            if (key == "")
                throw new ArgumentException($"Key was empty.", nameof(key));

            try
            {
                if (_dictionary.ContainsKey(key)) // if dictionary already contains key
                    _dictionary[key] = obj; // replace old object with new object
                else
                    _dictionary.Add(key, obj); // else add object
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
            if (!_dictionary.ContainsKey(key))
                throw new ArgumentException($"Object with key '{key}' was not found in storage.", nameof(key));

            try
            {
                return (T)_dictionary[key];
            }
            catch (Exception ex)
            {
                throw new MemoryStorageException($"Couldn't restore object of type '{typeof(T).Name}' with key '{key}'.", ex);
            }
        }
    }
}
