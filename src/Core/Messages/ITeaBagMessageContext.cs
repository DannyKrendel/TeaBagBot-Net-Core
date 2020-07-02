using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.Messages
{
    public interface ITeaBagMessageContext
    {
        string AuthorUsername { get; }
        string AuthorMention { get; }
        string GuildName { get; }
        string ChannelName { get; }
        IReadOnlyCollection<string> RoleNames { get; }
    }
}
