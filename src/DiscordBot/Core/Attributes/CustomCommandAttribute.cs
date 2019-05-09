using Discord.Commands;
using System;

namespace DiscordBot.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CustomCommandAttribute : CommandAttribute
    {
        public CustomCommandAttribute(string name) :
            base(Unity.Resolve<CommandManager>().GetCommand(name).Name)
        {
        }
    }
}
