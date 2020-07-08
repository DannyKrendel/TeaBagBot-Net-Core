using Discord;
using MongoDB.Driver;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;
using TeaBagBot.Helpers;

namespace TeaBagBot.Services
{
    public class TeaBagCommandService
    {
        public IReadOnlyCollection<TeaBagCommand> Commands { get; }

        private readonly IRepository<TeaBagCommand> _commandRepository;
        private readonly IRepository<BotConfig> _configRepository;

        public TeaBagCommandService(IRepository<TeaBagCommand> commandRepository, IRepository<BotConfig> configRepository)
        {
            _commandRepository = commandRepository;
            _configRepository = configRepository;
            Commands = commandRepository.AsQueryable().ToList();
        }

        public IReadOnlyCollection<TeaBagCommand> GetCommandsInGroup(ModuleGroup group)
        {
            return Commands.Where(c => c.Group == group).ToList();
        }

        public TeaBagCommand GetCommand(string name)
        {
            return Commands.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
        }

        public static TeaBagCommand GetCommand<TCommandRepository>(string name) 
            where TCommandRepository : IRepository<TeaBagCommand>
        {
            var commandRepo = GenericUtils.CreateConstructor<TCommandRepository>(typeof(IMongoClient),typeof(IMongoDbSettings))
                (new MongoClient(), new SettingsService(new FileSystem()).Load().MongoDbSettings) as IRepository<TeaBagCommand>;

            return commandRepo.FindOne(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<GuildPermission> GetPermissions(string commandName)
        {
            var config = await _configRepository.FindOneAsync(x => x.CommandPermissions.FirstOrDefault(c => c.Command == commandName) != null);
            if (config == null)
                return 0;
            return config.CommandPermissions.FirstOrDefault(c => c.Command == commandName).Permissions;
        }

        public static GuildPermission GetPermissions<TConfigRepository>(string commandName)
            where TConfigRepository : IRepository<BotConfig>
        {
            var configRepo = GenericUtils.CreateConstructor<TConfigRepository>(typeof(IMongoClient), typeof(IMongoDbSettings))
                (new MongoClient(),new SettingsService(new FileSystem()).Load().MongoDbSettings) as IRepository<BotConfig>;

            foreach (var config in configRepo.AsQueryable().ToList())
            {
                foreach (var item in config.CommandPermissions)
                {
                    if (item.Command == commandName)
                        return item.Permissions;
                }
            }
            return 0;
        }

        public async Task ChangeCommandPermissions(string name, int permissions)
        {
            var config = await _configRepository.FindOneAsync(x => x.CommandPermissions.FirstOrDefault(c => c.Command == name) != null);
            if (config == null)
                return;
            config.CommandPermissions.FirstOrDefault(c => c.Command == name).Permissions = (GuildPermission)permissions;
            await _configRepository.ReplaceOneAsync(config);
        }
    }
}
