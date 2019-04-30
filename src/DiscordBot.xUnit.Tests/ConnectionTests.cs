using DiscordBot.Core;
using System.Threading.Tasks;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class ConnectionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("FakeToken")]
        public async Task ConnectAsync_ShouldThrowHttpException_IfInvalidToken(string token)
        {
            var connection = Unity.Resolve<Connection>();

            var exception = await Record.ExceptionAsync(async () => await connection.ConnectAsync(token));

            Assert.IsType<Discord.Net.HttpException>(exception);
        }
    }
}
