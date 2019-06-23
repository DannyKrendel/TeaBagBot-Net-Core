using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    public class DiscordMessageService
    {
        private readonly DiscordSocketClient _client;

        public DiscordMessageService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task SendMessageAsync(ulong channelId, string message)
        {
            await (_client.GetChannel(channelId) as IMessageChannel).SendMessageAsync(message);
        }

        public async Task SendMessageAsync(string channelName, string message)
        {
            foreach (var guild in _client.Guilds)
            {
                foreach (var channel in guild.Channels)
                {
                    if (channel.Name == channelName)
                    {
                        await (channel as IMessageChannel).SendMessageAsync(message);
                        return;
                    }
                }
            }
        }
    }
}
