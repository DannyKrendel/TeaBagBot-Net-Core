using TeaBagBot.Core.Entities;
using TeaBagBot.Core.Storage;
using System;
using System.Collections.Generic;

namespace TeaBagBot.Core.Commands
{
    public class CommandEntityService
    {
        private readonly IDataStorage _storage;

        public CommandEntityService(IDataStorage storage)
        {
            _storage = storage;
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
