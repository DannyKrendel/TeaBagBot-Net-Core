using DiscordBot.Core;
using DiscordBot.Core.Entities;
using System.Threading.Tasks;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void ConnectAsync_ShouldThrow()
        {
            var connection = Unity.Resolve<Connection>();

            Assert.ThrowsAsync<Discord.Net.HttpException>(
                () => connection.ConnectAsync(new BotConfig { Token = "FakeToken" }));
        }
    }
}
