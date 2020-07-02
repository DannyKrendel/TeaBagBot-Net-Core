using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TeaBagBot.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TeaBagBot.Commands;
using TeaBagBot.Services;
using Serilog;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;

namespace TeaBagBot.Commands
{
    public class CommandHandler : ICommandHandler<SocketMessage>
    {
        private ILogger _logger;
        private readonly LoggingService _loggingService;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IRepository<BotConfig> _configRepository;
        private readonly IServiceProvider _services;

        public CommandHandler(ILogger logger, LoggingService loggingService, DiscordSocketClient client, 
            CommandService commandService, IRepository<BotConfig> configRepository, IServiceProvider services)
        {
            _logger = logger;
            _loggingService = loggingService;
            _client = client;
            _commandService = commandService;
            _configRepository = configRepository;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _commandService.CommandExecuted += OnCommandExecutedAsync;
            await _commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
        }

        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            await _loggingService.LogCommandResultAsync(command, context, result);
        }

        public async Task HandleCommandAsync(SocketMessage message)
        {
            BotConfig config;
            try
            {
                config = await _configRepository.FindOneAsync(
                    c => c.GuildId == (message.Channel as SocketGuildChannel).Guild.Id);
            }
            catch (Exception ex)
            {
                _logger.Error("{ex}", ex);
                return;
            }

            var msg = message as SocketUserMessage;
            int argPos = 0;

            if (msg.HasStringPrefix(config.Prefix, ref argPos))
            {
                var context = new SocketCommandContext(_client, msg);
                await _commandService.ExecuteAsync(context, argPos, _services);
            }
        }
    }
}
