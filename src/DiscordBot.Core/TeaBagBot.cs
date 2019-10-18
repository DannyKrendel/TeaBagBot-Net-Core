using Discord.WebSocket;
using TeaBagBot.Core.Commands;
using TeaBagBot.Core.Logging;
using TeaBagBot.Core.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TeaBagBot.Core
{
    public class TeaBagBot : IBot
    {
        private readonly DiscordLogger _logger;
        private readonly Connection _connection;
        private readonly ICommandHandler<SocketMessage> _commandHandler;
        private readonly TokenService _tokenService;
        private readonly DataStorageService _dataStorageService;

        private CancellationTokenSource _cancelTokenSource;

        public TeaBagBot(DiscordLogger logger, Connection connection, 
            ICommandHandler<SocketMessage> commandHandler, TokenService tokenService, DataStorageService dataStorageService)
        {
            _logger = logger;
            _connection = connection;
            _commandHandler = commandHandler;
            _tokenService = tokenService;
            _dataStorageService = dataStorageService;
        }

        public async Task StartAsync()
        {
            _cancelTokenSource = new CancellationTokenSource();

            try
            {
                string token = _tokenService.GetToken();
                await _connection.ConnectAsync(token);
                await _commandHandler.InitializeAsync();
            }
            catch (TokenException ex)
            {
                await _logger.LogErrorAsync("Discord", ex);
                await StopAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Discord", ex);
                await StopAsync();
            }
            finally
            {
                await Task.Delay(-1, _cancelTokenSource.Token);
            }
        }

        public async Task StopAsync()
        {
            _dataStorageService.SaveEverythingToJson();
            await _connection.DisconnectAsync();

            if (_cancelTokenSource is null)
                return;
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();
            _cancelTokenSource = null;
        }
    }
}
