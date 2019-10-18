using System.Collections.Generic;
using System.Linq;

namespace TeaBagBot.Core.Entities
{
    public class TeaBagCommandGroup
    {
        public PermissionGroup Group { get; set; }
        public IReadOnlyCollection<TeaBagCommand> Commands { get; set; }

        public TeaBagCommandGroup(PermissionGroup group, IEnumerable<TeaBagCommand> commands)
        {
            Group = group;
            Commands = commands.ToList();
        }
    }
}
