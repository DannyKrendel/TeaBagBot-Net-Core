using DiscordBot.Storage.Implementations;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class JsonStorageTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test/test")]
        public void StoreObject_ShouldNotCreateFile_IfInvalidPath(string path)
        {
            var mockFileSystem = new MockFileSystem();
            var storage = new JsonStorage(mockFileSystem, Unity.Resolve<ILogger>());

            storage.StoreObject("test", path);

            Assert.False(mockFileSystem.FileExists($"{path}.json"));
        }

        [Theory]
        [InlineData(@"C:\test")]
        public void StoreObject_ShouldCreateFile_IfValidPath(string path)
        {
            var mockFileSystem = new MockFileSystem();
            var storage = new JsonStorage(mockFileSystem, Unity.Resolve<ILogger>());

            storage.StoreObject("data", path);

            Assert.True(mockFileSystem.FileExists($"{path}.json"));
        }

        [Theory]
        [InlineData("test", "\"test\"")]
        public void StoreObject_FileShouldContainExpected_IfValidData(string data, string expected)
        {
            var mockFileSystem = new MockFileSystem();
            var storage = new JsonStorage(mockFileSystem, Unity.Resolve<ILogger>());

            storage.StoreObject(data, @"C:\test");

            Assert.Equal(mockFileSystem.File.ReadAllText(@"C:\test.json"), expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RestoreObject_ShouldReturnNull_IfInvalidPath(string path)
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("\"test\"");
            mockFileSystem.AddFile(@"C:\test.json", mockInputFile);
            var storage = new JsonStorage(mockFileSystem, Unity.Resolve<ILogger>());

            var actual = storage.RestoreObject<string>(path);

            Assert.Null(actual);
        }

        [Theory]
        [InlineData("\"test\"", "test")]
        public void RestoreObject_ShouldReturnExpected_IfValidInput(string jsonData, string expected)
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(jsonData);
            mockFileSystem.AddFile(@"C:\test.json", mockInputFile);
            var storage = new JsonStorage(mockFileSystem, Unity.Resolve<ILogger>());

            var actual = storage.RestoreObject<string>(@"C:\test");

            Assert.Equal(expected, actual);
        }
    }
}
