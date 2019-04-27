using Discord.WebSocket;
using DiscordBot.Core;
using DiscordBot.Storage.Interfaces;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class UnityTests
    {
        [Fact]
        public void ResolveIDataStorage_ShouldWork()
        {
            var storage1 = Unity.Resolve<IDataStorage>();
            var storage2 = Unity.Resolve<IDataStorage>();

            Assert.NotNull(storage1);
            Assert.NotNull(storage2);
            Assert.Same(storage1, storage2);
        }

        [Fact]
        public void ResolveILogger_ShouldWork()
        {
            var storage1 = Unity.Resolve<ILogger>();
            var storage2 = Unity.Resolve<ILogger>();

            Assert.NotNull(storage1);
            Assert.NotNull(storage2);
            Assert.Same(storage1, storage2);
        }

        [Fact]
        public void ResolveDiscordSocketClient_ShouldWork()
        {
            var storage1 = Unity.Resolve<DiscordSocketClient>();
            var storage2 = Unity.Resolve<DiscordSocketClient>();

            Assert.NotNull(storage1);
            Assert.NotNull(storage2);
            Assert.Same(storage1, storage2);
        }

        [Fact]
        public void ResolveConnection_ShouldWork()
        {
            var storage1 = Unity.Resolve<Connection>();
            var storage2 = Unity.Resolve<Connection>();

            Assert.NotNull(storage1);
            Assert.NotNull(storage2);
            Assert.Same(storage1, storage2);
        }
    }
}
