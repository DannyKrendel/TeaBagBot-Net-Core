using DiscordBot.Core.Entities;
using DiscordBot.Storage.Interfaces;
using System;

namespace DiscordBot
{
    public static class ConfigManager
    {
        private static readonly string configPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Config\Config";

        public static BotConfig LoadConfig()
        {
            return Unity.Resolve<IDataStorage>().RestoreObject<BotConfig>(configPath);
        }

        public static void SaveConfig(BotConfig config)
        {
            try
            {
                Unity.Resolve<IDataStorage>().StoreObject(config, configPath);
            }
            catch (Exception ex)
            {
                throw new ConfigException($"Path to config was incorrect. Fix it in '{nameof(ConfigManager)}'.", ex);
            }
        }
    }
}
