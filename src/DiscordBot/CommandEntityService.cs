using DiscordBot.Core.Entities;
using DiscordBot.Storage.Interfaces;
using System;
using System.Collections.Generic;

namespace DiscordBot
{
    public class CommandEntityService
    {
        private readonly IDataStorage storage;

        public CommandEntityService(IDataStorage storage)
        {
            this.storage = storage;
        }

        public IEnumerable<CommandGroup> LoadCommands()
        {
            try
            {
                return storage.RestoreObject<IEnumerable<CommandGroup>>("CommandGroups");
            }
            catch (Exception ex)
            {
                throw new CommandException($"Couldn't load commands.", ex);
            }
        }

        public void SaveCommands(IEnumerable<CommandGroup> commandGroups)
        {
            try
            {
                storage.StoreObject(commandGroups, "CommandGroups");
            }
            catch (Exception ex)
            {
                throw new CommandException($"Couldn't save commands.", ex);
            }
        }
    }
}
