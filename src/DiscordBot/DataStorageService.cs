using DiscordBot.Core;
using DiscordBot.Core.Entities;
using DiscordBot.Storage.Implementations;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiscordBot
{
    public class DataStorageService
    {
        private readonly JsonStorage jsonStorage;
        private readonly MemoryStorage memoryStorage;

        private readonly string tokenPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Config\Token";
        private readonly string configPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Config\Config";
        private readonly string commandsPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Commands";

        public DataStorageService(JsonStorage jsonStorage, MemoryStorage memoryStorage)
        {
            this.jsonStorage = jsonStorage;
            this.memoryStorage = memoryStorage;
        }

        public void LoadEverythingToMemory()
        {
            var token = jsonStorage.RestoreObject<string>(tokenPath);
            memoryStorage.StoreObject(token, "Token");

            var config = jsonStorage.RestoreObject<BotConfig>(configPath);
            memoryStorage.StoreObject(config, "Config");

            var commandGroups = new List<CommandGroup>();
            var groupNames = Enum.GetNames(typeof(PermissionGroup));

            foreach (var groupName in groupNames)
            {
                var groupPath = Path.Combine(commandsPath, groupName);

                if (!Directory.Exists(groupPath))
                {
                    throw new IOException($"Directory doesn't exist. Path: '{groupPath}'.");
                }

                var paths = Directory.GetFiles(groupPath);

                var commands = new List<CommandData>();

                foreach (var path in paths)
                {
                    var command = jsonStorage.RestoreObject<CommandData>(path);
                    commands.Add(command);
                }

                PermissionGroup group = (PermissionGroup)Enum.Parse(typeof(PermissionGroup), groupName);
                commandGroups.Add(new CommandGroup(group, commands));
            }

            memoryStorage.StoreObject(commandGroups, "CommandGroups");
        }

        public void SaveEverythingToJson()
        {
            var config = memoryStorage.RestoreObject<BotConfig>("Config");
            jsonStorage.StoreObject(config, configPath);

            var commandGroups = memoryStorage.RestoreObject<IEnumerable<CommandGroup>>("CommandGroups");

            foreach (var commandGroup in commandGroups)
            {
                var groupPath = Path.Combine(commandsPath, commandGroup.Group.ToString());
                foreach (var command in commandGroup.Commands)
                {
                    jsonStorage.StoreObject(command, Path.Combine(groupPath, command.Name + ".json"));
                }
            }
        }
    }
}
