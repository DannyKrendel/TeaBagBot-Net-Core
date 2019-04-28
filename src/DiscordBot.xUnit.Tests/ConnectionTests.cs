using Discord;
using Discord.WebSocket;
using DiscordBot.Core;
using DiscordBot.Core.Entities;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public async Task ConnectAsync_ShouldThrow()
        {
            var connection = Unity.Resolve<Connection>();

            await Assert.ThrowsAsync<Discord.Net.HttpException>(
                async () => await connection.ConnectAsync(new BotConfig { Token = "FakeToken" }));
        }
    }
}
