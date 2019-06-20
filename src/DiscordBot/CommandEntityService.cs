using DiscordBot.Core.Entities;
using DiscordBot.Storage;
using System;
using System.Collections.Generic;

namespace DiscordBot
{
    public class CommandEntityService
    {
        private readonly IDataStorage _storage;

        public CommandEntityService(IDataStorage storage)
        {
            this._storage = storage;
        }

        public IEnumerable<CommandGroup> LoadCommands()
        {
            try
            {
                return _storage.RestoreObject<IEnumerable<CommandGroup>>("CommandGroups");
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
                _storage.StoreObject(commandGroups, "CommandGroups");
            }
            catch (Exception ex)
            {
                throw new CommandException($"Couldn't save commands.", ex);
            }
        }
    }
}
