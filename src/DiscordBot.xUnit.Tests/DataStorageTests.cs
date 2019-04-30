using DiscordBot.Storage.Implementations;
using DiscordBot.Storage.Interfaces;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class DataStorageTests
    {
        [Fact]
        public void DefaultDataStorage_ShouldBeJsonStorage()
        {
            var storage = Unity.Resolve<IDataStorage>();

            Assert.True(storage is JsonStorage);
        }
    }
}
