using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core;
using DiscordBot.Core.Logging;
using DiscordBot.Storage;
using System.IO.Abstractions;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class UnityTests
    {
        [Fact]
        public void RegisterTypes_ShouldNotThrow()
        {
            var ex = Record.Exception(() => Unity.RegisterTypes());

            Assert.Null(ex);
        }

        [Fact]
        public void Resolve_ShouldReturnSingleton()
        {
            Unity.Resolve<DataStorageService>().LoadEverythingToMemory();

            AssertResolvedTypeIsSingleton<IFileSystem>();
            AssertResolvedTypeIsSingleton<IDataStorage>();
            AssertResolvedTypeIsSingleton<ILogger>();
            AssertResolvedTypeIsSingleton<DiscordLogger>();
            AssertResolvedTypeIsSingleton<DiscordSocketClient>();
            AssertResolvedTypeIsSingleton<CommandService>();
            AssertResolvedTypeIsSingleton<Connection>();
            AssertResolvedTypeIsSingleton<EmbedService>();
            AssertResolvedTypeIsSingleton<CommandEntityService>();
            AssertResolvedTypeIsSingleton<CommandManager>();
            AssertResolvedTypeIsSingleton<CommandHandler>();
            AssertResolvedTypeIsSingleton<DiscordBot>();
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
