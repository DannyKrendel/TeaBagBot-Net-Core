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
            this._client = client;
        }

        public async Task SendMessageAsync(ulong channelId, string message)
        {
            var channel = _client.GetChannel(channelId) as IMessageChannel;
            await channel.SendMessageAsync(message);
        }

        public async Task SendMessageAsync(string channelName, string message)
        {
            throw new NotImplementedException();
        }
    }
}
