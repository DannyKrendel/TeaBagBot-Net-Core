using DiscordBot.Core.Entities;
using DiscordBot.Storage.Interfaces;
using System;

namespace DiscordBot
{
    public class ConfigService
    {
        private readonly IDataStorage storage;

        public ConfigService(IDataStorage storage)
        {
            this.storage = storage;
        }

        public BotConfig LoadConfig()
        {
            try
            {
                return storage.RestoreObject<BotConfig>("Config");
            }
            catch (Exception ex)
            {
                throw new ConfigException($"Couldn't load config.", ex);
            }
        }

        public void SaveConfig(BotConfig config)
        {
            try
            {
                storage.StoreObject(config, "Config");
            }
            catch (Exception ex)
            {
                throw new ConfigException($"Couldn't save config.", ex);
            }
        }
    }
}
