using Discord.Commands;

namespace TeaBagBot.Core.Helpers
{
    public class CommandParser
    {
        public string Parse(string str, ICommandContext context)
        {
            string result = str;

            if (str.Contains("$username"))
            {
                result = str.Replace("$username", context.User.Username);
            }
            else if (str.Contains("$usermention"))
            {
                result = str.Replace("$usermention", context.User.Mention);
            }

            return result;
        }
    }
}
