using Discord;
using Discord.WebSocket;
using DiscordBot.Core.Logging;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    public class Connection
    {
        private readonly DiscordLogger _logger;
        private readonly DiscordSocketClient _client;

        public Connection(DiscordLogger logger, DiscordSocketClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task ConnectAsync(string token)
        {
            _client.Log += _logger.LogAsync;

            try
            {
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
            }
            catch (Exception ex)
            {
                throw new ConnectionException("Something went wrong while trying to connect.", ex);
            }
        }

        public async Task DisconnectAsync()
        {
            await _client.LogoutAsync();
            await _client.StopAsync();

            _client.Log -= _logger.LogAsync;
        }
    }
}
