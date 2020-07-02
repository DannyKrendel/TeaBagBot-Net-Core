using System.Collections.Generic;
using System.Linq;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;

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

        public IReadOnlyCollection<TeaBagCommand> GetCommands(PermissionGroup group)
        {
            return Commands.Where(c => c.PermissionGroup == group).ToList();
        }

        public TeaBagCommand GetCommand(string name, PermissionGroup? group = null)
        {
            if (group.HasValue)
            {
                return Commands.FirstOrDefault(c => c.Name.ToLower() == name.ToLower() && c.PermissionGroup == group);
            }
            else
            {
                return Commands.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            }
        }
    }
}
