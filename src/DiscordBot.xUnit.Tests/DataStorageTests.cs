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
            Assert.Throws<ArgumentException>(() => storage.RestoreObject<string>("doesn't exist"));
        }
    }
}
