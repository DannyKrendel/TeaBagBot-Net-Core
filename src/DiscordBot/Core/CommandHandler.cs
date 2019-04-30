using Discord.Commands;
using Discord.WebSocket;
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
            if (socketMsg == null)
                throw new ArgumentException($"{nameof(socketMsg)} cannot be null.");
            var msg = socketMsg as SocketUserMessage;
            if (msg == null)
                throw new InvalidCastException($"{nameof(socketMsg)} cannot be null.");

            int argPos = 0;

            if (!msg.HasStringPrefix(ConfigManager.LoadConfig().Prefix, ref argPos) ||
                msg.HasMentionPrefix(client.CurrentUser, ref argPos) ||
                msg.Author.IsBot)
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
