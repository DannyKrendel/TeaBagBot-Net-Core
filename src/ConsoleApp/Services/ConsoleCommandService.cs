using TeaBagBot.ConsoleApp.Commands.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TeaBagBot.ConsoleApp.Commands;
using Serilog;

namespace TeaBagBot.ConsoleApp.Services
{
    public class ConsoleCommandService
    {
        public event Func<string, Task> Log;

        internal ConsoleCommandParser Parser { get; }
        internal readonly ILogger _logger;
        private readonly ConsoleCommandBuilder _commandBuilder;

        internal ConsoleCommandService(ILogger logger, ConsoleCommandBuilder commandBuilder, ConsoleCommandParser parser)
        {
            Parser = parser;
            _logger = logger;
            _commandBuilder = commandBuilder;
        }

        public async Task AddModulesAsync(Assembly assembly, IServiceProvider services)
        {
            var modules = await ConsoleCommandBuilder.SearchModulesAsync(assembly, this).ConfigureAwait(false);

            _commandBuilder.AddCommands(modules, this, services);
        }

        internal async Task ExecuteAsync(IConsoleCommandContext context, IServiceProvider services)
        {
            ConsoleCommandInfo command = null;
            object[] args = null;
            string name = new string(context.Message.TakeWhile(c => c != ' ').ToArray());

            if (name.Length == 0)
                return;

            string input = context.Message;
            if (context.Message.Contains(' '))
                input = context.Message.Substring(context.Message.IndexOf(' '));

            var foundCommands = SearchCommands(name);

            if (foundCommands.Length == 0)
            {
                _logger.Warning("Неизвестная команда.");
                return;
            }

            //string[] args = splittedMsg.TakeLast(splittedMsg.Length - 1).ToArray();

            foreach (var foundCommand in foundCommands)
            {
                var parameters = foundCommand.Parameters;

                var parseRes = await foundCommand.ParseAsync(context, input);

                if (parseRes.IsSuccess)
                {
                    command = foundCommand;
                    args = parseRes.ArgValues.ToArray();
                    break;
                }
            }

            if (command == null)
            {
                _logger.Warning("Неверные параметры.");
            }
            else
            {
                await command.ExecuteAsync(context, args, services);
            }
        }

        private ConsoleCommandInfo[] SearchCommands(string name)
        {
            var cmdList = new List<ConsoleCommandInfo>();

            foreach (var cmd in _commandBuilder.Commands)
            {
                if (cmd.Name == name || cmd.Aliases != null && cmd.Aliases.Contains(name))
                {
                    cmdList.Add(cmd);
                }
            }
            return cmdList.ToArray();
        }
    }
}
