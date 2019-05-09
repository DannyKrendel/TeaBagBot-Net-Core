using Discord.Commands;

namespace DiscordBot.Core.Attributes
{
    public class CustomAliasAttribute : AliasAttribute
    {
        public CustomAliasAttribute(string name) :
            base(Unity.Resolve<CommandManager>().GetCommand(name).Aliases)
        {
        }
    }
}
