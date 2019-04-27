using DiscordBot.Storage.Implementations;
using DiscordBot.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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

        [Fact]
        public void MemoryStorage_ShouldWork()
        {
            string expectedObj = "";
            string expectedKey = "";

            IDataStorage storage = new MemoryStorage();

            storage.StoreObject(expectedObj + "some string", expectedKey);
            storage.StoreObject(expectedObj, expectedKey);

            var actualObj = storage.RestoreObject<string>(expectedKey);

            Assert.Equal(expectedObj, actualObj);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MemoryStorage_ShouldThrow(string key)
        {
            IDataStorage storage = new MemoryStorage();

            Assert.ThrowsAny<Exception>(() => storage.RestoreObject<string>(key));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", null)]
        public void JsonStorage_StoreObject_ShouldThrow(object obj, string key)
        {
            var storage = Unity.Resolve<IDataStorage>();

            Assert.ThrowsAny<Exception>(() => storage.StoreObject(obj, key));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void JsonStorage_RetoreObject_ShouldThrow(string key)
        {
            var storage = Unity.Resolve<IDataStorage>();

            Assert.ThrowsAny<Exception>(() => storage.RestoreObject<object>(key));
        }
    }
}
