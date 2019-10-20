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
        private readonly string _responsesPath = @"C:\Users\Danny\Source\Repos\TeaBagBot\Responses";

        public DataStorageService(JsonStorage jsonStorage, MemoryStorage memoryStorage)
        {
            _jsonStorage = jsonStorage;
            _memoryStorage = memoryStorage;
        }

        public void LoadEverythingToMemory()
        {
            var token = _jsonStorage.RestoreObject<string>(_tokenPath);
            _memoryStorage.StoreObject(token, "Token");

            var config = _jsonStorage.RestoreObject<TeaBagConfig>(_configPath);
            _memoryStorage.StoreObject(config, "Config");

            var commandGroups = new List<TeaBagCommandGroup>();
            var groupNames = Enum.GetNames(typeof(PermissionGroup));

            foreach (var groupName in groupNames)
            {
                var groupPath = Path.Combine(_commandsPath, groupName);

                if (!Directory.Exists(groupPath))
                {
                    throw new IOException($"Directory doesn't exist. Path: '{groupPath}'.");
                }

                var paths = Directory.GetFiles(groupPath);

                var commands = new List<TeaBagCommand>();

                foreach (var path in paths)
                {
                    var command = _jsonStorage.RestoreObject<TeaBagCommand>(path);
                    commands.Add(command);
                }

                PermissionGroup group = (PermissionGroup)Enum.Parse(typeof(PermissionGroup), groupName);
                commandGroups.Add(new TeaBagCommandGroup(group, commands));
            }

            _memoryStorage.StoreObject(commandGroups.AsReadOnly(), "CommandGroups");

            var rPaths = Directory.GetFiles(_responsesPath);

            var responses = new List<TeaBagResponse>();

            foreach (var path in rPaths)
            {
                var response = _jsonStorage.RestoreObject<TeaBagResponse>(path);
                responses.Add(response);
            }

            _memoryStorage.StoreObject(responses.AsReadOnly(), "Responses");
        }

        public void SaveEverythingToJson()
        {
            var config = _memoryStorage.RestoreObject<TeaBagConfig>("Config");
            _jsonStorage.StoreObject(config, _configPath);

            var commandGroups = _memoryStorage.RestoreObject<IReadOnlyCollection<TeaBagCommandGroup>>("CommandGroups");

            foreach (var commandGroup in commandGroups)
            {
                var path = Path.Combine(_commandsPath, commandGroup.Group.ToString());
                foreach (var command in commandGroup.Commands)
                {
                    _jsonStorage.StoreObject(command, Path.Combine(path, command.Name + ".json"));
                }
            }

            var responses = _memoryStorage.RestoreObject<IReadOnlyCollection<TeaBagResponse>>("Responses");

            foreach (var response in responses)
            {
                var path = Path.Combine(_responsesPath, response.Name);
                _jsonStorage.StoreObject(response, path);
            }
        }
    }
}
