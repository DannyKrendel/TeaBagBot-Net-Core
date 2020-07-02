using System.Threading.Tasks;
using Xunit;
using TeaBagBot.ConsoleApp.DI;
using TeaBagBot;
using System;
using Moq;
using TeaBagBot.Services;
using Serilog;
using Discord.WebSocket;

namespace TeaBagBot.UnitTests
{
    public class ConnectionTests
    {
        private readonly ILogger _logger;
        private readonly LoggingService _loggingService;
        private readonly DiscordSocketClient _client;

        public ConnectionTests()
        {
            _logger = Mock.Of<ILogger>();
            _loggingService = new LoggingService(_logger, Mock.Of<EmbedService>());
            _client = new DiscordSocketClient();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ConnectAsync_InvalidToken_ShouldThrow(string token)
        {
            var connection = new Connection(_logger, _loggingService, _client);

            var exception = await Record.ExceptionAsync(async () => await connection.ConnectAsync(token)); 

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task DisconnectAsync_ShouldNotThrow()
        {
            var connection = new Connection(_logger, _loggingService, _client);

            var exception = await Record.ExceptionAsync(async () => await connection.DisconnectAsync());

            Assert.Null(exception);
        }
    }
}
