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
        private readonly ILogger logger;
        private readonly DiscordSocketClient client;
        private readonly CommandService service;

        public CommandHandler(ILogger logger, DiscordSocketClient client, CommandService service)
        {
            this.logger = logger;
            this.client = client;
            this.service = service;
        }

        public async Task InitializeAsync()
        {
            await service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            client.MessageReceived += HandleMessageAsync;
        }

        public async Task HandleMessageAsync(SocketMessage socketMsg)
        {
            BotConfig config = ConfigManager.LoadConfig(); // this should throw

            var msg = socketMsg as SocketUserMessage;
            int argPos = 0;

            if (!msg.HasStringPrefix(config.Prefix, ref argPos) &&
                !msg.HasMentionPrefix(client.CurrentUser, ref argPos))
                return;

            var context = new SocketCommandContext(client, msg);
            var result = await service.ExecuteAsync(context, argPos, null);

            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
            {
                logger.Log(result.ErrorReason);
            }
        }
    }
}
