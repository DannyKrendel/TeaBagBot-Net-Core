using DiscordBot.Storage.Implementations;
using DiscordBot.Storage.Interfaces;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class DataStorageTests
    {
        [Fact]
        public void DefaultStorageIsJsonStorage_ShouldWork()
        {
            var storage = Unity.Resolve<IDataStorage>();

            Assert.True(storage is JsonStorage);
        }

        [Theory]
        [InlineData("", "")]
        public void MemoryStorage_RestoreObject_ShouldWork(string expectedObj, string expectedKey)
        {
            IDataStorage storage = new MemoryStorage();

            storage.StoreObject(expectedObj + "some string", expectedKey);
            storage.StoreObject(expectedObj, expectedKey);

            var actualObj = storage.RestoreObject<string>(expectedKey);

            Assert.Equal(expectedObj, actualObj);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MemoryStorage_RestoreObject_ShouldThrow(string key)
        {
            IDataStorage storage = new MemoryStorage();

            Assert.ThrowsAny<Exception>(() => storage.RestoreObject<string>(key));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", null)]
        public void JsonStorage_StoreObject_ShouldThrow(object obj, string path)
        {
            var storage = Unity.Resolve<IDataStorage>();

            Assert.ThrowsAny<Exception>(() => storage.StoreObject(obj, path));
        }

        [Theory]
        [InlineData(@"C:\test", "test", "\"test\"")]
        public void JsonStorage_StoreObject_ShouldWork(string path, string data, string expected)
        {
            var mockFileSystem = new MockFileSystem();

            var storage = new JsonStorage(mockFileSystem);

            storage.StoreObject(data, path);

            Assert.True(Path.IsPathFullyQualified(path));
            Assert.True(mockFileSystem.FileExists($"{path}.json"));
            Assert.Equal(mockFileSystem.File.ReadAllText($"{path}.json"), expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void JsonStorage_RestoreObject_ShouldThrow(string path)
        {
            var storage = Unity.Resolve<IDataStorage>();

            Assert.ThrowsAny<Exception>(() => storage.RestoreObject<object>(path));
        }

        [Theory]
        [InlineData(@"C:\test", "\"test\"", "test")]
        public void JsonStorage_RestoreObject_ShouldWork(string path, string jsonData, string expected)
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(jsonData);
            mockFileSystem.AddFile($"{path}.json", mockInputFile);

            var storage = new JsonStorage(mockFileSystem);
            var actual = storage.RestoreObject<string>(path);

            Assert.True(Path.IsPathFullyQualified(path));
            Assert.Equal(expected, actual);
        }
    }
}
