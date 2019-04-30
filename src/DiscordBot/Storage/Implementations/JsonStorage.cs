using DiscordBot.Extensions;
using DiscordBot.Storage.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO.Abstractions;

namespace DiscordBot.Storage.Implementations
{
    public class JsonStorage : IDataStorage
    {
        private readonly IFileSystem fileSystem;
        private readonly ILogger logger;

        public JsonStorage(IFileSystem fileSystem, ILogger logger)
        {
            this.fileSystem = fileSystem;
            this.logger = logger;
        }

        public T RestoreObject<T>(string path)
        {
            T result = default;

            try
            {
                if (!fileSystem.Path.IsPathFullyQualified(path))
                    throw new ArgumentException("Invalid path.", nameof(path));

                string json = fileSystem.File.ReadAllText($"{path}.json");
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                logger.Log($"Object can't be restored. {ex.Message}");
            }

            return result;
        }

        public void StoreObject(object obj, string path)
        {
            string file = $"{path}.json";

            try
            {
                if (!fileSystem.Path.IsPathFullyQualified(path))
                    throw new ArgumentException("Invalid path.", nameof(path));

                fileSystem.Directory.CreateDirectory(fileSystem.Path.GetDirectoryName(file));
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                fileSystem.File.WriteAllText(file, json);
            }
            catch (Exception ex)
            {
                logger.Log($"Object can't be stored. {ex.Message}");
            }
        }
    }
}
