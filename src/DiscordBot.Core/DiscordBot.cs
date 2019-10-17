using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Console;
using DiscordBot.Console.Commands;
using DiscordBot.Core;
using DiscordBot.Core.Attributes;
using DiscordBot.Core.Logging;
using DiscordBot.Core.Modules;
using DiscordBot.Storage;
using DiscordBot.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class DiscordBot : IBot
    {
        private readonly DiscordLogger _logger;
        private readonly Connection _connection;
        private readonly ICommandHandler<SocketMessage> _commandHandler;
        private readonly ICommandHandler<string> _consoleHandler;

        private readonly CancellationTokenSource _cancelTokenSource;

        public DiscordBot(DiscordLogger logger, Connection connection, ICommandHandler<SocketMessage> commandHandler, ICommandHandler<string> consoleHandler)
        {
            _logger = logger;
            _connection = connection;
            _commandHandler = commandHandler;
            _consoleHandler = consoleHandler;
        }

        public async Task StartAsync()
        {
            string token = "";

            try
            {
                token = Unity.Resolve<TokenService>().GetToken();
            }
            catch (TokenException ex)
            {
                await _logger.LogErrorAsync("Discord", ex);
            }

            AttributeUtils.TryLoadAttributes(
                new Type[] { typeof(StandardModule), typeof(AdminModule) },
                new Type[] { typeof(CustomAliasAttribute), typeof(CustomCommandAttribute) });

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
                await StopAsync();
            }
        }

        public async Task StopAsync()
        {
            Unity.Resolve<DataStorageService>().SaveEverythingToJson();
            await _connection.DisconnectAsync();
        }
    }
}
