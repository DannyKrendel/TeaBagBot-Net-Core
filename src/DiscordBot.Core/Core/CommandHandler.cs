using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Core.Entities;
using DiscordBot.Core.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    public class CommandHandler : ICommandHandler<SocketMessage>
    {
        private readonly DiscordLogger _logger;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordLogger logger, DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _logger = logger;
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandleMessageAsync;
            _commands.CommandExecuted += OnCommandExecutedAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            await _logger.LogCommandResultAsync(command, context, result);
        }

        public async Task HandleMessageAsync(SocketMessage socketMsg)
        {
            BotConfig config;
            try
            {
                config = Unity.Resolve<ConfigService>().LoadConfig();
            }
            catch (ConfigException ex)
            {
                await _logger.LogErrorAsync("Command", ex);
                return;
            }

            var msg = socketMsg as SocketUserMessage;
            int argPos = 0;

            if (!msg.HasStringPrefix(config.Prefix, ref argPos) &&
                !msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                return;

            var context = new SocketCommandContext(_client, msg);
            await _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}
