using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TeaBagBot.Commands;
using TeaBagBot.ConsoleApp.Services;

namespace TeaBagBot.ConsoleApp.Commands
{
    public class ConsoleCommandHandler : ICommandHandler<string>
    {
        private readonly ILogger _logger;
        private readonly ConsoleCommandService _commandService;
        private readonly IServiceProvider _services;

        public ConsoleCommandHandler(ILogger logger, ConsoleCommandService commandService, IServiceProvider services)
        {
            _logger = logger;
            _commandService = commandService;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            do
            {
                string msg = Console.ReadLine();
                await HandleCommandAsync(msg);
            } while (true);
        }

        public async Task HandleCommandAsync(string msg)
        {
            var context = new ConsoleCommandContext(msg);
            await _commandService.ExecuteAsync(context, _services);
        }
    }
}
