using DiscordBot.Core;
using DiscordBot.Core.Logging;
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
                AttributeUtilities.TryLoadAttributes();

                string token = Unity.Resolve<TokenService>().GetToken();
                var connect = connection.ConnectAsync(token);
                var command = commandHandler.InitializeAsync();
                await connect.ContinueWith(async t => await command);
            }
            catch (Exception ex)
            {
                await connection.DisconnectAsync();
                await logger.LogErrorAsync("Discord", ex);
                Unity.Resolve<DataStorageService>().SaveEverythingToJson();
            }
        }
    }
}
