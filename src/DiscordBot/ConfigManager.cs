using DiscordBot.Core.Entities;
using DiscordBot.Storage.Interfaces;

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
            Unity.Resolve<IDataStorage>().StoreObject(config, configPath);
        }
    }
}
