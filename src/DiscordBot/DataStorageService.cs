using DiscordBot.Core.Entities;
using DiscordBot.Storage.Implementations;
using System.Collections.Generic;

namespace DiscordBot
{
    public class DataStorageService
    {
        private readonly JsonStorage jsonStorage;
        private readonly MemoryStorage memoryStorage;

        private readonly string tokenPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Config\Token";
        private readonly string configPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Config\Config";
        private readonly string commandsPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Config\Commands";

        public DataStorageService(JsonStorage jsonStorage, MemoryStorage memoryStorage)
        {
            this.jsonStorage = jsonStorage;
            this.memoryStorage = memoryStorage;
        }

        public void LoadEverythingToMemory()
        {
            var token = jsonStorage.RestoreObject<string>(tokenPath);
            var config = jsonStorage.RestoreObject<BotConfig>(configPath);
            var commands = jsonStorage.RestoreObject<List<CommandEntity>>(commandsPath);

            memoryStorage.StoreObject(token, "Token");
            memoryStorage.StoreObject(config, "Config");
            memoryStorage.StoreObject(commands, "Commands");
        }

        public void SaveEverythingToJson()
        {
            var config = memoryStorage.RestoreObject<BotConfig>("Config");
            var commands = memoryStorage.RestoreObject<List<CommandEntity>>("Commands");
            jsonStorage.StoreObject(config, configPath);
            jsonStorage.StoreObject(commands, commandsPath);
        }
    }
}
