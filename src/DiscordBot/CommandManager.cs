using DiscordBot.Core;
using DiscordBot.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot
{
    public class CommandManager
    {
        public List<CommandGroup> CommandGroups { get; }

        public CommandManager(CommandEntityService commandEntityService)
        {
            CommandGroups = commandEntityService.LoadCommands().ToList();
        }

        public IEnumerable<CommandData> GetCommands(PermissionGroup group)
        {
            return CommandGroups.First(cg => cg.Group == group).Commands;
        }

        public CommandData GetCommand(string name, PermissionGroup? group = null)
        {
            if (group.HasValue)
            {
                var g = CommandGroups.FirstOrDefault(cg => cg.Group == group);
                if (g.Commands.Count() != 0)
                {
                    return g.Commands.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
                }
            }
            else
            {
                foreach (var g in CommandGroups)
                {
                    if (g.Commands.Count() != 0)
                    {
                        CommandData command = g.Commands.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
                        if (command != null)
                        {
                            return command;
                        }
                    }
                }
            }
            return null;
        }
    }
}
