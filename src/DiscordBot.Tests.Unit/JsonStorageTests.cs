using TeaBagBot.Core.Storage.Json;
using System;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class JsonStorageTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test/test")]
        public void StoreObject_ShouldThrowArgumentException_IfInvalidPath(string path)
        {
            var mockFileSystem = new MockFileSystem();
            var storage = new JsonStorage(mockFileSystem);

            var exception = Record.Exception(() => storage.StoreObject("test", path));

            Assert.IsAssignableFrom<ArgumentException>(exception);
        }

        [Theory]
        [InlineData(@"C:\test")]
        public void StoreObject_ShouldCreateFile_IfValidPath(string path)
        {
            var mockFileSystem = new MockFileSystem();
            var storage = new JsonStorage(mockFileSystem);

            storage.StoreObject("data", path);

            Assert.True(mockFileSystem.FileExists($"{path}.json"));
        }

        [Theory]
        [InlineData("test", "\"test\"")]
        public void StoreObject_FileShouldContainExpected_IfValidData(string data, string expected)
        {
            var mockFileSystem = new MockFileSystem();
            var storage = new JsonStorage(mockFileSystem);

            storage.StoreObject(data, @"C:\test");

            Assert.Equal(mockFileSystem.File.ReadAllText(@"C:\test.json"), expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RestoreObject_ShouldThrowArgumentException_IfInvalidPath(string path)
        {
            var mockFileSystem = new MockFileSystem();
            var storage = new JsonStorage(mockFileSystem);

            var exception = Record.Exception(() => storage.RestoreObject<string>(path));

            Assert.IsAssignableFrom<ArgumentException>(exception);
        }

        [Theory]
        [InlineData(@"C:\test")]
        [InlineData(@"C:\fakePath\test")]
        public void RestoreObject_ShouldThrowJsonStorageException_IfFileDoesntExist(string path)
        {
            var mockFileSystem = new MockFileSystem();
            var storage = new JsonStorage(mockFileSystem);

            var exception = Record.Exception(() => storage.RestoreObject<string>(path));

            Assert.IsType<JsonStorageException>(exception);
        }

        [Theory]
        [InlineData("\"test\"", "test")]
        public void RestoreObject_ShouldReturnExpected_IfValidInput(string jsonData, string expected)
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(jsonData);
            mockFileSystem.AddFile(@"C:\test.json", mockInputFile);
            var storage = new JsonStorage(mockFileSystem);

            var actual = storage.RestoreObject<string>(@"C:\test");

            Assert.Equal(expected, actual);
        }
    }
}
