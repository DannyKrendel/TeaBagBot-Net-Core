using DiscordBot.Core;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class DiscordBot
    {
        private readonly DiscordLogger logger;
        private readonly Connection connection;
        private readonly CommandHandler commandHandler;

        public DiscordBot(DiscordLogger logger, Connection connection, CommandHandler commandHandler)
        {
            this.logger = logger;
            this.connection = connection;
            this.commandHandler = commandHandler;
        }

        public async Task StartAsync()
        {
            try
            {
                string token = Unity.Resolve<TokenService>().GetToken();
                await connection.ConnectAsync(token);
                await commandHandler.InitializeAsync();
            }
            catch (Exception ex)
            {
                await connection.DisconnectAsync();
                await logger.LogException(nameof(DiscordBot), ex);
                Unity.Resolve<DataStorageService>().SaveEverythingToJson();
            }
        }
    }
}
