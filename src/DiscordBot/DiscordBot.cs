using DiscordBot.ConsoleUtils;
using DiscordBot.Core;
using DiscordBot.Core.Logging;
using DiscordBot.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class DiscordBot
    {
        private readonly DiscordLogger _logger;
        private readonly Connection _connection;
        private readonly CommandHandler _commandHandler;
        private readonly ConsoleCommandHandler _consoleHandler;

        private readonly CancellationTokenSource _cancelTokenSource;

        public DiscordBot(DiscordLogger logger, Connection connection, CommandHandler commandHandler, ConsoleCommandHandler consoleHandler)
        {
            _logger = logger;
            _connection = connection;
            _commandHandler = commandHandler;
            _consoleHandler = consoleHandler;
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
                await _logger.LogErrorAsync("Discord", ex);
            }

            AttributeUtils.TryLoadAttributes();

            try
            {
                var connect = _connection.ConnectAsync(token);
                var command = _commandHandler.InitializeAsync();
                await connect.ContinueWith(async t => await command);

                await _consoleHandler.InitializeAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Discord", ex);
            }
            finally
            {
                await ExitAsync();
            }

            //do
            //{
            //    exit = restart = false;
            //    try
            //    {

            //        AttributeUtilities.TryLoadAttributes();

            //        var connect = connection.ConnectAsync(token);
            //        var command = commandHandler.InitializeAsync();
            //        await connect.ContinueWith(async t => await command);

            //        do
            //        {
            //            var consoleCommand = await consoleHandler.InitializeAsync();

            //            switch (consoleCommand.Item1)
            //            {
            //                case ConsoleCommand.Exit:
            //                    exit = true;
            //                    break;
            //                case ConsoleCommand.Restart:
            //                    restart = true;
            //                    break;
            //                case ConsoleCommand.Say:
            //                    await discordMessages.SendMessageAsync(320211476201734144, consoleCommand.Item2);
            //                    break;
            //                default:
            //                    await logger.LogWarningAsync("Console", "Неверная консольная команда.");
            //                    break;
            //            }
            //        } while (exit == false && restart == false);
            //    }
            //    catch (Exception ex)
            //    {
            //        await logger.LogErrorAsync("Discord", ex);
            //    }
            //    finally
            //    {
            //        Unity.Resolve<DataStorageService>().SaveEverythingToJson();
            //        await connection.DisconnectAsync();
            //    }
            //} while (restart);
        }

        public async Task ExitAsync()
        {
            Unity.Resolve<DataStorageService>().SaveEverythingToJson();
            await _connection.DisconnectAsync();
        }
    }
}
