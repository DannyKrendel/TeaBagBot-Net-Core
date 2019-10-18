using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace TeaBagBot.Core.Messages
{
    public class TeaBagMessageContext : ITeaBagMessageContext
    {
        public string AuthorUsername { get; }

        public string AuthorMention { get; }

        public string ChannelName { get; }

        public string GuildName { get; }

        public IReadOnlyCollection<string> RoleNames { get; }

        public TeaBagMessageContext(IMessage message)
        {
            AuthorUsername = message.Author.Username;
            AuthorMention = message.Author.Mention;
            ChannelName = message.Channel.Name;
            GuildName = (message.Author as SocketGuildUser).Guild.Name;
            RoleNames = (message.Author as SocketGuildUser).Guild.Roles.Select(r => r.Name).ToList();
        }
    }
}
