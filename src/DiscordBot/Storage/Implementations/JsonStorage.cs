using System.IO.Abstractions;
using DiscordBot.Storage.Interfaces;
using Newtonsoft.Json;

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
            var json = fileSystem.File.ReadAllText($"{path}.json");
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void StoreObject(object obj, string path)
        {
            var file = $"{path}.json";
            fileSystem.Directory.CreateDirectory(fileSystem.Path.GetDirectoryName(file));
            var json = JsonConvert.SerializeObject(obj);
            fileSystem.File.WriteAllText(file, json);
        }
    }
}
