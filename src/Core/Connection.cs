using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Serilog;
using TeaBagBot.Services;

namespace TeaBagBot
{
    public class Connection
    {
        private readonly ILogger _logger;
        private readonly LoggingService _loggingService;
        private readonly DiscordSocketClient _client;

        public Connection(ILogger logger, LoggingService loggingService, DiscordSocketClient client)
        {
            _logger = logger;
            _loggingService = loggingService;
            _client = client;
        }

        public async Task ConnectAsync(string token)
        {
            _client.Log += _loggingService.LogFromMessageAsync;

            try
            {
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при подключении.", ex);
            }
        }

        public async Task DisconnectAsync()
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
            _client.Log -= _loggingService.LogFromMessageAsync;
        }
    }
}
