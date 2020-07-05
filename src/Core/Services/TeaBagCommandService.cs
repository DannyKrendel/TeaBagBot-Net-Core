using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;
using TeaBagBot.Helpers;

namespace TeaBagBot.Services
{
    public class TeaBagCommandService
    {
        public IReadOnlyCollection<TeaBagCommand> Commands { get; }

        private readonly IRepository<TeaBagCommand> _commandRepository;

        public TeaBagCommandService(IRepository<TeaBagCommand> commandRepository)
        {
            _commandRepository = commandRepository;
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

        public static TeaBagCommand GetCommand<T>(string name) where T : IRepository<TeaBagCommand>
        {
            var constructor = GenericUtils.CreateConstructor<T>(typeof(MongoDbSettings));
            var commandRepo = constructor(new SettingsService(new FileSystem()).Load().MongoDbSettings) as IRepository<TeaBagCommand>;
            
            return commandRepo.FindOne(c => c.Name.ToLower() == name.ToLower());
        }
    }
}
