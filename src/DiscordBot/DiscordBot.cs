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
        private readonly ConsoleHandler consoleHandler;

        public DiscordBot(DiscordLogger logger, Connection connection, CommandHandler commandHandler, ConsoleHandler consoleHandler)
        {
            this.logger = logger;
            this.connection = connection;
            this.commandHandler = commandHandler;
            this.consoleHandler = consoleHandler;
        }

        public async Task StartAsync()
        {
            try
            {
                AttributeUtilities.TryLoadAttributes();

                string token = Unity.Resolve<TokenService>().GetToken();
                var connect = connection.ConnectAsync(token);
                var command = commandHandler.InitializeAsync();
                var console = consoleHandler.CheckMessagesAsync();
                await connect.ContinueWith(async t => await command);

                await console;
            }
            catch (Exception ex)
            {
                await logger.LogErrorAsync("Discord", ex);
            }
            finally
            {
                await connection.DisconnectAsync();
                Unity.Resolve<DataStorageService>().SaveEverythingToJson();
            }
        }
    }
}
