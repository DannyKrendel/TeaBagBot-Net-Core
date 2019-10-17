using DiscordBot.Storage;
using DiscordBot.Storage.Memory;
using System;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class MemoryStorageTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void StoreObject_ShouldThrowArgumentException_IfInvalidKey(string key)
        {
            IDataStorage storage = new MemoryStorage();

            var exception = Record.Exception(() => storage.StoreObject("test", key));

            Assert.IsAssignableFrom<ArgumentException>(exception);
        }

        [Theory]
        [InlineData("value", "key")]
        public void StoreObject_ShouldContainExpected_IfValidKey(object expected, string key)
        {
            IDataStorage storage = new MemoryStorage();

            storage.StoreObject(expected, key);

            var actualObj = storage.RestoreObject<string>(key);
            Assert.Equal(expected, actualObj);
        }

        [Theory]
        [InlineData("previous", "expected", "key")]
        public void StoreObject_ShouldReplacePreviousWithExpected_IfValidKey(object previous, object expected, string key)
        {
            IDataStorage storage = new MemoryStorage();

            storage.StoreObject(previous, key);
            storage.StoreObject(expected, key);

            var actual = storage.RestoreObject<string>(key);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RestoreObject_ShouldThrowArgumentException_IfInvalidKey(string key)
        {
            IDataStorage storage = new MemoryStorage();

            var exception = Record.Exception(() => storage.RestoreObject<string>(key));

            Assert.IsAssignableFrom<ArgumentException>(exception);
        }

        [Theory]
        [InlineData("key")]
        public void RestoreObject_ShouldThrowArgumentException_IfObjectWasNotFound(string key)
        {
            IDataStorage storage = new MemoryStorage();

            var exception = Record.Exception(() => storage.RestoreObject<object>(key));

            Assert.IsAssignableFrom<ArgumentException>(exception);
        }

        [Theory]
        [InlineData("obj", "key")]
        public void RestoreObject_ShouldThrowMemoryStorageException_IfWrongCast(object obj, string key)
        {
            IDataStorage storage = new MemoryStorage();
            storage.StoreObject(obj, key);

            var exception = Record.Exception(() => storage.RestoreObject<int>(key));

            Assert.IsType<MemoryStorageException>(exception);
        }
    }
}
