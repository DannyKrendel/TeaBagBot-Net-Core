using Discord.WebSocket;
using DiscordBot.Core;
using DiscordBot.Storage.Interfaces;
using System.IO.Abstractions;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class UnityTests
    {
        [Fact]
        public void ResolveSingleton_ShouldWork()
        {
            AssertResolvedTypeIsSingleton<IDataStorage>();
            AssertResolvedTypeIsSingleton<ILogger>();
            AssertResolvedTypeIsSingleton<DiscordSocketClient>();
            AssertResolvedTypeIsSingleton<Connection>();
            AssertResolvedTypeIsSingleton<DiscordBot>();
            AssertResolvedTypeIsSingleton<IFileSystem>();
        }

        private void AssertResolvedTypeIsSingleton<T>() where T : class
        {
            var t1 = Unity.Resolve<T>();
            var t2 = Unity.Resolve<T>();

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.Same(t1, t2);
        }
    }
}
