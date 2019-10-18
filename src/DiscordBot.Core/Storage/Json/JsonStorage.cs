using TeaBagBot.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.IO.Abstractions;

namespace TeaBagBot.Core.Storage.Json
{
    public class JsonStorage : IDataStorage
    {
        private readonly IFileSystem _fileSystem;

        public JsonStorage(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void StoreObject(object obj, string path)
        {
            ValidatePath(ref path);

            try
            {
                _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(path));
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                _fileSystem.File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                throw new JsonStorageException($"Couldn't store object '{nameof(obj)}' to '{path}'.", ex);
            }
        }

        public T RestoreObject<T>(string path)
        {
            ValidatePath(ref path);

            try
            {
                string json = _fileSystem.File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new JsonStorageException($"Couldn't restore object of type '{typeof(T)}' from '{path}'.", ex);
            }
        }

        private void ValidatePath(ref string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path), "Path was null.");

            if (!_fileSystem.Path.IsPathFullyQualified(path))
                throw new ArgumentException($"Invalid path: '{path}'. It must by fully qualified.", nameof(path));

            if (!path.EndsWith(".json"))
                path = $"{path}.json";
        }
    }
}
