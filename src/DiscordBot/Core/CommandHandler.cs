using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core.Entities;
using DiscordBot.Core.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    public class CommandHandler
    {
        private readonly DiscordLogger logger;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly IServiceProvider services;

        public CommandHandler(DiscordLogger logger, DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            this.logger = logger;
            this.client = client;
            this.commands = commands;
            this.services = services;
        }

        public async Task InitializeAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            commands.CommandExecuted += OnCommandExecutedAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            await logger.LogCommandResultAsync(command, context, result);
        }

        public async Task HandleCommandAsync(SocketMessage socketMsg)
        {
            BotConfig config;
            try
            {
                config = Unity.Resolve<ConfigService>().LoadConfig();
            }
            catch (ConfigException ex)
            {
                await logger.LogErrorAsync("Command", ex);
                return;
            }

            var msg = socketMsg as SocketUserMessage;
            int argPos = 0;

            if (!msg.HasStringPrefix(config.Prefix, ref argPos) &&
                !msg.HasMentionPrefix(client.CurrentUser, ref argPos))
                return;

            var context = new SocketCommandContext(client, msg);
            await commands.ExecuteAsync(context, argPos, services);
        }
    }
}
