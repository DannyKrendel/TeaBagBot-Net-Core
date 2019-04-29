using DiscordBot.Storage.Implementations;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class JsonStorageTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("", null)]
        public void JsonStorage_StoreObject_ShouldThrow(object obj, string path)
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("\"test\"");
            mockFileSystem.AddFile(@"C:\test.json", mockInputFile);
            var storage = new JsonStorage(mockFileSystem);

            var exception = Record.Exception(() => storage.StoreObject(obj, path));

            Assert.NotNull(exception);
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
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("\"test\"");
            mockFileSystem.AddFile(@"C:\test.json", mockInputFile);
            var storage = new JsonStorage(mockFileSystem);

            var exception = Record.Exception(() => storage.RestoreObject<string>(path));

            Assert.NotNull(exception);
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
