using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Core;
using DiscordBot.Core.Logging;
using DiscordBot.Storage;
using System.IO.Abstractions;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class UnityTests
    {
        [Fact]
        public void RegisterTypes_ShouldNotThrow()
        {
            var ex = Record.Exception(() => Unity.RegisterTypes());

            Assert.Null(ex);
        }
    }
}
