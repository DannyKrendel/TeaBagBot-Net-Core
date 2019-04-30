using DiscordBot.Extensions;
using DiscordBot.Storage.Interfaces;
using DiscordBot.Storage.Exceptions;
using Newtonsoft.Json;
using System;
using System.IO.Abstractions;

namespace DiscordBot.Storage.Implementations
{
    public class JsonStorage : IDataStorage
    {
        private readonly IFileSystem fileSystem;

        public JsonStorage(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public T RestoreObject<T>(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path), "Path was null.");

            if (!fileSystem.Path.IsPathFullyQualified(path))
                throw new ArgumentException($"Invalid path: '{path}'. It must by fully qualified.", nameof(path));

            try
            {
                string json = fileSystem.File.ReadAllText($"{path}.json");
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new JsonStorageException($"Couldn't restore object of type '{typeof(T).Name}' from '{path}'.", ex);
            }
        }

        public void StoreObject(object obj, string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path), "Path was null.");

            if (!fileSystem.Path.IsPathFullyQualified(path))
                throw new ArgumentException($"Invalid path: '{path}'. It must by fully qualified.", nameof(path));

            string file = $"{path}.json";

            try
            {
                fileSystem.Directory.CreateDirectory(fileSystem.Path.GetDirectoryName(file));
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                fileSystem.File.WriteAllText(file, json);
            }
            catch (Exception ex)
            {
                throw new JsonStorageException($"Couldn't store object '{nameof(obj)}' to '{path}'.", ex);
            }
        }
    }
}
