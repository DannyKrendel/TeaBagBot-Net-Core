using Discord;
using Discord.WebSocket;
using DiscordBot.Core.Entities;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    public class Connection
    {
        private readonly DiscordSocketClient client;
        private readonly DiscordLogger logger;

        public Connection(DiscordLogger logger, DiscordSocketClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task ConnectAsync(string token)
        {
            client.Log += logger.Log;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(1000);
        }
    }
}
