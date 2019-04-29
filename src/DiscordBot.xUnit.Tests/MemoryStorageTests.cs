using DiscordBot.Storage.Implementations;
using DiscordBot.Storage.Interfaces;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class MemoryStorageTests
    {
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

            var exception = Record.Exception(() => storage.RestoreObject<string>(key));

            Assert.NotNull(exception);
        }
    }
}
