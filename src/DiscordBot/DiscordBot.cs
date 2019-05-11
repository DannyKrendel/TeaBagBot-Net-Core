using DiscordBot.ConsoleUtilities;
using DiscordBot.Core;
using DiscordBot.Core.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class DiscordBot
    {
        private readonly DiscordLogger logger;
        private readonly Connection connection;
        private readonly CommandHandler commandHandler;
        private readonly ConsoleHandler consoleHandler;

        private readonly CancellationTokenSource cancelTokenSource;

        public DiscordBot(DiscordLogger logger, Connection connection, CommandHandler commandHandler, ConsoleHandler consoleHandler)
        {
            this.logger = logger;
            this.connection = connection;
            this.commandHandler = commandHandler;
            this.consoleHandler = consoleHandler;
        }

        public async Task StartAsync()
        {
            bool exit;
            bool restart;
            string token = "";

            try
            {
                token = Unity.Resolve<TokenService>().GetToken();
            }
            catch (TokenException ex)
            {
                await logger.LogErrorAsync("Discord", ex);
            }

            do
            {
                exit = restart = false;
                try
                {

                    AttributeUtilities.TryLoadAttributes();

                    var connect = connection.ConnectAsync(token);
                    var command = commandHandler.InitializeAsync();
                    await connect.ContinueWith(async t => await command);

                    do
                    {
                        var consoleCommand = await consoleHandler.CheckMessagesAsync();
                        if (!consoleCommand.Item1.HasValue)
                        {
                            await logger.LogWarningAsync("Console", "Неверная консольная команда.");
                        }
                        else
                        {
                            switch (consoleCommand.Item1.Value)
                            {
                                case ConsoleCommand.Exit:
                                    exit = true;
                                    break;
                                case ConsoleCommand.Restart:
                                    restart = true;
                                    break;
                                case ConsoleCommand.Say:
                                    break;
                            }
                        }
                    } while (exit == false && restart == false);
                }
                catch (Exception ex)
                {
                    await logger.LogErrorAsync("Discord", ex);
                }
                finally
                {
                    Unity.Resolve<DataStorageService>().SaveEverythingToJson();
                    await connection.DisconnectAsync();
                }
            } while (restart);
        }
    }
}
