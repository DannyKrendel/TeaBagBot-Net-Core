using Discord.WebSocket;
using TeaBagBot.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using TeaBagBot.Messages;
using TeaBagBot.Services;
using Serilog;
using TeaBagBot.Models;

namespace TeaBagBot
{
    public class TeaBagBot : IBot
    {
        private readonly ILogger _logger;
        private readonly AppSettings _settings;
        private readonly Connection _connection;
        private readonly ICommandHandler<SocketMessage> _commandHandler;
        private readonly IMessageHandler<SocketMessage> _messageHandler;
        private CancellationTokenSource _cancelTokenSource;

        public TeaBagBot(ILogger logger, AppSettings settings, Connection connection,
            ICommandHandler<SocketMessage> commandHandler, IMessageHandler<SocketMessage> messageHandler)
        {
            _logger = logger;
            _settings = settings;
            _connection = connection;
            _commandHandler = commandHandler;
            _messageHandler = messageHandler;
        }

        public async Task StartAsync()
        {
            _cancelTokenSource = new CancellationTokenSource();

            try
            {
                string token = _settings.DiscordToken;
                await _connection.ConnectAsync(token);
                await _commandHandler.InitializeAsync();
                await _messageHandler.InitializeAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("[Discord] {ex}", ex);
                await StopAsync();
            }
            //finally
            //{
            //    await Task.Delay(-1, _cancelTokenSource.Token);
            //}
        }

        public async Task StopAsync()
        {
            await _connection.DisconnectAsync();

            if (_cancelTokenSource is null)
                return;
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();
            _cancelTokenSource = null;
        }
    }
}
