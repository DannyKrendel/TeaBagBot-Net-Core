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

        public List<CommandEntity> LoadCommands()
        {
            try
            {
                return storage.RestoreObject<List<CommandEntity>>("Commands");
            }
            catch (Exception ex)
            {
                throw new CommandException($"Couldn't load commands.", ex);
            }
        }

        public void SaveCommands(List<CommandEntity> commands)
        {
            try
            {
                storage.StoreObject(commands, "Commands");
            }
            catch (Exception ex)
            {
                throw new CommandException($"Couldn't save commands.", ex);
            }
        }
    }
}
