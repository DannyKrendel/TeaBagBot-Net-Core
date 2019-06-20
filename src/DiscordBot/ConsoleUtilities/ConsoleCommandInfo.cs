using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.ConsoleUtilities
{
    public class ConsoleCommandInfo
    {
        public string Name { get; }
        public string Summary { get; }
        public IReadOnlyList<string> Aliases { get; }
        public IReadOnlyList<string> Parameters { get; }

        private readonly Func<ConsoleCommandContext, object[], IServiceProvider, ConsoleCommandInfo, Task> _action;

        public ConsoleCommandInfo(string name, string summary, string[] aliases, string[] parameters,
            Func<ConsoleCommandContext, object[], IServiceProvider, ConsoleCommandInfo, Task> action)
        {
            Name = name;
            Summary = summary;
            Aliases = aliases;
            Parameters = parameters;
            _action = action;
        }

        public async Task ExecuteAsync(ConsoleCommandContext context, object[] args, IServiceProvider services)
        {
            await _action(context, args, services, this);
        }
    }
}
