namespace TeaBagBot.Core.Messages
{
    public class ResponseParser
    {
        public string Parse(string message, ITeaBagMessageContext context)
        {
            string result = message;

            if (message.Contains("$AuthorUsername$"))
            {
                result = message.Replace("$AuthorUsername$", context.AuthorUsername);
            }
            else if (message.Contains("$AuthorMention$"))
            {
                result = message.Replace("$AuthorMention$", context.AuthorMention);
            }
            else if (message.Contains("$GuildName$"))
            {
                result = message.Replace("$GuildName$", context.GuildName);
            }
            else if (message.Contains("$ChannelName$"))
            {
                result = message.Replace("$ChannelName$", context.ChannelName);
            }

            return result;
        }
    }
}
