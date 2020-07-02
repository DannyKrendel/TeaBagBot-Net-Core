using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeaBagBot.Services
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
            await SendMessageAsync(_client.GetChannel(channelId) as IMessageChannel, message);
        }

        public async Task SendMessageAsync(string channelName, string message)
        {
            foreach (var guild in _client.Guilds)
            {
                foreach (var channel in guild.Channels)
                {
                    if (channel.Name == channelName)
                    {
                        await SendMessageAsync(channel as IMessageChannel, message);
                        return;
                    }
                }
            }
        }

        private async Task SendMessageAsync(IMessageChannel channel, string message)
        {
            var messageEmotes = Regex.Matches(message, @":\S+?:").Select(m => m.Value).ToArray();
            var guildEmotes = (channel as IGuildChannel).Guild.Emotes;
            for (int i = 0; i < messageEmotes.Length; i++)
            {
                var foundEmote = guildEmotes.FirstOrDefault(e => messageEmotes[i] == $":{e.Name}:");
                if (foundEmote != null)
                {
                    message = message.Replace(messageEmotes[i], foundEmote.ToString());
                }
            }
            await channel.SendMessageAsync(message);
        }
    }
}
