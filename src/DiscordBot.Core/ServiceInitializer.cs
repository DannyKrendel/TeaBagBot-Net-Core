using Discord.WebSocket;
using TeaBagBot.Core.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using TeaBagBot.Core.Helpers;

namespace TeaBagBot.Core
{
    public class ServiceInitializer
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly CommandManager _commandManager;
        private readonly ConfigService _configService;
        private readonly CommandParser _commandParser;
        private readonly DiscordMessageService _messageService;

        public ServiceInitializer(DiscordSocketClient client, EmbedService embedService,
            CommandManager commandManager, ConfigService config, CommandParser commandParser, DiscordMessageService messageService)
        {
            _client = client;
            _embedService = embedService;
            _commandManager = commandManager;
            _configService = config;
            _commandParser = commandParser;
            _messageService = messageService;
        }

        public IServiceProvider BuildServices() => new ServiceCollection()
            .AddSingleton(_embedService)
            .AddSingleton(_commandManager)
            .AddSingleton(_client)
            .AddSingleton(_configService)
            .AddSingleton(_commandParser)
            .AddSingleton(_messageService)
            .BuildServiceProvider();
    }
}
