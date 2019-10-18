using TeaBagBot.Core.Entities;
using TeaBagBot.Core.Storage.Json;
using TeaBagBot.Core.Storage.Memory;
using System;
using System.Collections.Generic;
using System.IO;

namespace TeaBagBot.Core.Storage
{
    public class DataStorageService
    {
        private readonly JsonStorage _jsonStorage;
        private readonly MemoryStorage _memoryStorage;

        private readonly string _tokenPath = @"C:\Users\Danny\Source\Repos\TeaBagBot\Config\Token";
        private readonly string _configPath = @"C:\Users\Danny\Source\Repos\TeaBagBot\Config\Config";
        private readonly string _commandsPath = @"C:\Users\Danny\Source\Repos\TeaBagBot\CommandDatabase";

        public DataStorageService(JsonStorage jsonStorage, MemoryStorage memoryStorage)
        {
            _jsonStorage = jsonStorage;
            _memoryStorage = memoryStorage;
        }

        public void LoadEverythingToMemory()
        {
            var token = _jsonStorage.RestoreObject<string>(_tokenPath);
            _memoryStorage.StoreObject(token, "Token");

            var config = _jsonStorage.RestoreObject<BotConfig>(_configPath);
            _memoryStorage.StoreObject(config, "Config");

            var commandGroups = new List<CommandGroup>();
            var groupNames = Enum.GetNames(typeof(PermissionGroup));

            foreach (var groupName in groupNames)
            {
                var groupPath = Path.Combine(_commandsPath, groupName);

                if (!Directory.Exists(groupPath))
                {
                    throw new IOException($"Directory doesn't exist. Path: '{groupPath}'.");
                }

                var paths = Directory.GetFiles(groupPath);

                var commands = new List<CommandData>();

                foreach (var path in paths)
                {
                    var command = _jsonStorage.RestoreObject<CommandData>(path);
                    commands.Add(command);
                }

                PermissionGroup group = (PermissionGroup)Enum.Parse(typeof(PermissionGroup), groupName);
                commandGroups.Add(new CommandGroup(group, commands));
            }

            _memoryStorage.StoreObject(commandGroups, "CommandGroups");
        }

        public void SaveEverythingToJson()
        {
            var config = _memoryStorage.RestoreObject<BotConfig>("Config");
            _jsonStorage.StoreObject(config, _configPath);

            var commandGroups = _memoryStorage.RestoreObject<IEnumerable<CommandGroup>>("CommandGroups");

            foreach (var commandGroup in commandGroups)
            {
                var groupPath = Path.Combine(_commandsPath, commandGroup.Group.ToString());
                foreach (var command in commandGroup.Commands)
                {
                    _jsonStorage.StoreObject(command, Path.Combine(groupPath, command.Name + ".json"));
                }
            }
        }
    }
}
