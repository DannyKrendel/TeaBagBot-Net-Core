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
            var logger1 = Unity.Resolve<ILogger>();
            var logger2 = Unity.Resolve<ILogger>();

            Assert.NotNull(logger1);
            Assert.NotNull(logger2);
            Assert.Same(logger1, logger2);
        }

        [Fact]
        public void ResolveDiscordSocketClient_ShouldWork()
        {
            var client1 = Unity.Resolve<DiscordSocketClient>();
            var client2 = Unity.Resolve<DiscordSocketClient>();

            Assert.NotNull(client1);
            Assert.NotNull(client2);
            Assert.Same(client1, client2);
        }

        [Fact]
        public void ResolveConnection_ShouldWork()
        {
            var con1 = Unity.Resolve<Connection>();
            var con2 = Unity.Resolve<Connection>();

            Assert.NotNull(con1);
            Assert.NotNull(con2);
            Assert.Same(con1, con2);
        }
    }
}
