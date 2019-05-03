using DiscordBot.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot
{
    public class CommandManager
    {
        private readonly List<CommandEntity> commands;

        public CommandManager(CommandEntityService commandEntityService)
        {
            commands = commandEntityService.LoadCommands();
        }

        public CommandEntity GetCommand(string name)
        {
            return commands.First(c => c.Name == name);
        }

        public void AddCommand(CommandEntity command)
        {
            commands.Add(command);
        }
    }
}
