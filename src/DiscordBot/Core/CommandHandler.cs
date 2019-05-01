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
            if (socketMsg == null)
                throw new ArgumentNullException(nameof(socketMsg), "Socket message was null.");
            var msg = socketMsg as SocketUserMessage;
            if (msg == null)
                throw new InvalidCastException($"Can't cast '{socketMsg.GetType()}' to '{typeof(SocketUserMessage)}'");

            int argPos = 0;

            BotConfig config = null;

            // This is here for debug purposes, and it does catch an exception
            try
            {
                config = ConfigManager.LoadConfig();
            }
            catch
            {
                throw;
            }

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
