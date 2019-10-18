using TeaBagBot.Core;
using TeaBagBot.ConsoleApp;
using System.Threading.Tasks;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class ConnectionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("FakeToken")]
        public async Task ConnectAsync_ShouldThrowConnectionException_IfInvalidToken(string token)
        {
            var connection = UnityDI.Resolve<Connection>();

            var exception = await Record.ExceptionAsync(async () => await connection.ConnectAsync(token));

            Assert.IsType<ConnectionException>(exception);
        }
    }
}
