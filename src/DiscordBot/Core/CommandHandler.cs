using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService service;
        private readonly ILogger logger;

        public CommandHandler(DiscordSocketClient client, CommandService service, ILogger logger)
        {
            this.client = client;
            this.service = service;
            this.logger = logger;
        }

        public async Task InitializeAsync()
        {
            await service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            client.MessageReceived += HandleMessage;
        }

        public async Task HandleMessage(SocketMessage socketMsg)
        {
            if (socketMsg == null)
                throw new ArgumentException($"{nameof(socketMsg)} cannot be null.");
            var msg = socketMsg as SocketUserMessage;
            if (msg == null)
                throw new InvalidCastException($"{nameof(socketMsg)} cannot be null.");

            var context = new SocketCommandContext(client, msg);

            int argPos = 0;

            if (msg.HasStringPrefix(ConfigManager.LoadConfig().Prefix, ref argPos) ||
                msg.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var result = await service.ExecuteAsync(context, argPos, null);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    logger.Log(result.ErrorReason);
                }
            }
        }
    }
}
