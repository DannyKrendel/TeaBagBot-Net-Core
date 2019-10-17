using System.Collections.Generic;

namespace DiscordBot.Core.Entities
{
    public class CommandGroup
    {
        public PermissionGroup Group { get; set; }
        public IEnumerable<CommandData> Commands { get; set; }

        public CommandGroup(PermissionGroup group, IEnumerable<CommandData> commands)
        {
            Group = group;
            Commands = commands;
        }
    }
}
