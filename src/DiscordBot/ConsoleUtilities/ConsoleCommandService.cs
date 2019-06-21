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
        public List<ConsoleCommandInfo> Commands { get; }

        public event Func<string, Task> Log;
        public event Func<ConsoleCommandInfo, ConsoleCommandContext, Task> CommandExecuted;

        private readonly ConsoleCommands _commandsModule;
        private readonly DiscordLogger _logger;

        public ConsoleCommandService(DiscordLogger logger)
        {
            Commands = new List<ConsoleCommandInfo>();
            _commandsModule = new ConsoleCommands();
            _logger = logger;
        }

        public void AddCommands()
        {
            foreach (var method in typeof(ConsoleCommands).GetMethods())
            {
                var command = BuildCommand(typeof(ConsoleCommands).GetTypeInfo(), method, null);

                if (command != null)
                    Commands.Add(command);
            }
        }

        public async Task ExecuteAsync(ConsoleCommandContext context, IServiceProvider services)
        {
            var splittedMsg = context.Message.Split(new char[] { ' ' });

            string name = splittedMsg[0];
            string[] args = splittedMsg.TakeLast(splittedMsg.Length - 1).ToArray();

            var command = SearchCommand(name);

            if (command == null)
            {
                await _logger.LogWarningAsync("Command", "Unknown command.");
                return;
            }

            await command.ExecuteAsync(context, args, services);
        }

        private ConsoleCommandInfo SearchCommand(string name)
        {
            foreach (var cmd in Commands)
            {
                if (cmd.Name == name || cmd.Aliases.Contains(name))
                {
                    return cmd;
                }
            }
            return null;
        }

        private ConsoleCommandInfo BuildCommand(TypeInfo typeInfo, MethodInfo method, IServiceProvider serviceprovider)
        {
            string[] parameters;
            var attr = method.GetCustomAttributes().FirstOrDefault(
                a => a.GetType() == typeof(ConsoleCommandAttribute)) as ConsoleCommandAttribute;
            if (attr != null)
            {
                parameters = method.GetParameters().Select(p => p.Name).ToArray();
            }
            else
            {
                return null;
            }

            async Task ExecuteCallback(ConsoleCommandContext context, object[] args, IServiceProvider services, ConsoleCommandInfo cmd)
            {
                _commandsModule.Context = context;

                var task = method.Invoke(_commandsModule, args) as Task ?? Task.Delay(0);

                await task.ConfigureAwait(false);
            }

            return new ConsoleCommandInfo(attr.Name, attr.Summary, attr.Aliases, parameters, ExecuteCallback);
        }
    }
}
