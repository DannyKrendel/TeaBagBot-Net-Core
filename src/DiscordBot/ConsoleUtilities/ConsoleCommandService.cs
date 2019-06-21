using DiscordBot.ConsoleUtilities.Attributes;
using DiscordBot.Core.Logging;
using DiscordBot.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.ConsoleUtilities
{
    public class ConsoleCommandService
    {
        public event Func<string, Task> Log;

        internal readonly DiscordLogger _logger;
        private readonly ConsoleCommandBuilder _commandBuilder;

        internal ConsoleCommandService(DiscordLogger logger, ConsoleCommandBuilder commandBuilder)
        {
            _logger = logger;
            _commandBuilder = commandBuilder;
        }

        public async Task AddModulesAsync(Assembly assembly, IServiceProvider services)
        {
            var modules = await ConsoleCommandBuilder.SearchModulesAsync(assembly, this).ConfigureAwait(false);

            _commandBuilder.AddCommands(modules);
        }

        internal async Task ExecuteAsync(ConsoleCommandContext context, IServiceProvider services)
        {
            var splittedMsg = context.Message.Split(new char[] { ' ' });

            string name = splittedMsg[0];
            string[] args = splittedMsg.TakeLast(splittedMsg.Length - 1).ToArray();

            var command = SearchCommand(name);

            if (command == null)
            {
                await _logger.LogWarningAsync("ConsoleCmd", "Unknown command.");
                return;
            }

            await command.ExecuteAsync(context, args, services);
        }

        private ConsoleCommandInfo SearchCommand(string name)
        {
            foreach (var cmd in _commandBuilder.Commands)
            {
                if (cmd.Name == name || cmd.Aliases.Contains(name))
                {
                    return cmd;
                }
            }
            return null;
        }
    }
}
