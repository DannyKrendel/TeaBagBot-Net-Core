using DiscordBot.Core.Logging;
using System;
using System.Threading.Tasks;

namespace DiscordBot.ConsoleUtilities
{
    public class ConsoleCommandHandler
    {
        private readonly ILogger _logger;
        private readonly ConsoleCommandService _commands;
        private readonly IServiceProvider _services;

        public ConsoleCommandHandler(ILogger logger, ConsoleCommandService commands, IServiceProvider services)
        {
            _logger = logger;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            _commands.AddCommands();
            do
            {
                string msg = Console.ReadLine();
                await HandleMessageAsync(msg);
            } while (true);
        }

        public async Task HandleMessageAsync(string msg)
        {
            var context = new ConsoleCommandContext(msg, this);
            await _commands.ExecuteAsync(context, _services);
        }
    }
}
