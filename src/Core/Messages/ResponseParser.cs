namespace TeaBagBot.Messages
{
    public class ResponseParser
    {
        public string Parse(string message, ITeaBagMessageContext context)
        {
            return message
                .Replace("$AuthorUsername$", context.AuthorUsername)
                .Replace("$AuthorMention$", context.AuthorMention)
                .Replace("$GuildName$", context.GuildName)
                .Replace("$ChannelName$", context.ChannelName);
        }
    }
}
