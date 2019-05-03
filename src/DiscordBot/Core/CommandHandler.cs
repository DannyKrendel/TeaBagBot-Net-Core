using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core.Entities;
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
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            client.MessageReceived += HandleCommandAsync;
        }

        public async Task HandleCommandAsync(SocketMessage socketMsg)
        {
            BotConfig config = null;

            try
            {
                config = Unity.Resolve<ConfigService>().LoadConfig();
            }
            catch (ConfigException ex)
            {
                await logger.LogException(nameof(CommandHandler), ex);
                return;
            }

            var msg = socketMsg as SocketUserMessage;
            int argPos = 0;

            if (!msg.HasStringPrefix(config.Prefix, ref argPos) &&
                !msg.HasMentionPrefix(client.CurrentUser, ref argPos))
                return;

            var context = new SocketCommandContext(client, msg);
            var result = await commands.ExecuteAsync(context, argPos, services);

            if (!result.IsSuccess)
            {
                await logger.LogCommandResult(result, context);
            }
        }
    }
}
