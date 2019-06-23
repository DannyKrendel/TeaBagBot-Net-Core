using DiscordBot.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.ConsoleUtils
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

            _commandBuilder.AddCommands(modules, this, services);
        }

        internal async Task ExecuteAsync(ConsoleCommandContext context, IServiceProvider services)
        {
            var splittedMsg = context.Message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (splittedMsg.Length == 0)
            {
                return;
            }

            string name = splittedMsg[0];
            string[] args = splittedMsg.TakeLast(splittedMsg.Length - 1).ToArray();

            ConsoleCommandInfo command = null;
            var foundCommands = SearchCommands(name);

            if (foundCommands.Length == 0)
            {
                await _logger.LogWarningAsync("ConsoleCmd", "Unknown command.");
                return;
            }
            // determine what command to choose if there are multiple found commands
            else if (foundCommands.Length > 1)
            {
                int paramsDiff = int.MaxValue;

                foreach (var cmd in foundCommands)
                {
                    int tempDiff = Math.Abs(cmd.Parameters.Count() - args.Length);

                    if (tempDiff < paramsDiff)
                    {
                        paramsDiff = tempDiff;
                        command = cmd;
                    }
                }
            }

            command = foundCommands[0];

            if (args.Length < command.Parameters.Count())
            {
                await _logger.LogWarningAsync("ConsoleCmd", "Not enough parameters.");
                return;
            }
            else if (args.Length > command.Parameters.Count())
            {
                await _logger.LogWarningAsync("ConsoleCmd", "Too many parameters.");
                return;
            }

            await command.ExecuteAsync(context, args, services);
        }

        private ConsoleCommandInfo[] SearchCommands(string name)
        {
            var cmdList = new List<ConsoleCommandInfo>();

            foreach (var cmd in _commandBuilder.Commands)
            {
                if (cmd.Name == name || cmd.Aliases.Contains(name))
                {
                    cmdList.Add(cmd);
                }
            }
            return cmdList.ToArray();
        }
    }
}
