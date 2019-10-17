using DiscordBot.Commands;
using DiscordBot.Core.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Console.Commands
{
    public class ConsoleCommandHandler : ICommandHandler<string>
    {
        private readonly DiscordLogger _logger;
        private readonly ConsoleCommandService _commands;
        private readonly IServiceProvider _services;

        public ConsoleCommandHandler(DiscordLogger logger, ConsoleCommandService commands, IServiceProvider services)
        {
            _logger = logger;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            do
            {
                string msg = System.Console.ReadLine();
                await HandleMessageAsync(msg);
            } while (true);
        }

        public async Task HandleMessageAsync(string msg)
        {
            var context = new ConsoleCommandContext(msg);
            await _commands.ExecuteAsync(context, _services);
        }
    }
}
