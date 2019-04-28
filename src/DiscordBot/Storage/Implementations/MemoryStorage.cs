using DiscordBot.Storage.Interfaces;
using System;
using System.Collections.Generic;

namespace DiscordBot.Storage.Implementations
{
    public class MemoryStorage : IDataStorage
    {
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public void StoreObject(object obj, string key)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = obj;
            else
                dictionary.Add(key, obj);
        }

        public T RestoreObject<T>(string key)
        {
            if (!dictionary.ContainsKey(key))
                throw new ArgumentException($"Ключ '{key}' не найден.");
            return (T)dictionary[key];
        }
    }
}
