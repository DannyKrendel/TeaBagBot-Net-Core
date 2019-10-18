using Discord.WebSocket;
using TeaBagBot.Core.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using TeaBagBot.Core.Messages;

namespace TeaBagBot.Core
{
    public class ServiceBuilder
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly TeaBagCommandProvider _commandManager;
        private readonly ConfigService _configService;
        private readonly ResponseParser _responseParser;
        private readonly DiscordMessageService _messageService;
        private readonly ResponseProvider _responseProvider;

        public ServiceBuilder(DiscordSocketClient client, EmbedService embedService,
            TeaBagCommandProvider commandManager, ConfigService config, ResponseParser responseParser, 
            DiscordMessageService messageService, ResponseProvider responseProvider)
        {
            _client = client;
            _embedService = embedService;
            _commandManager = commandManager;
            _configService = config;
            _responseParser = responseParser;
            _messageService = messageService;
            _responseProvider = responseProvider;
        }

        public IServiceProvider BuildServices() => new ServiceCollection()
            .AddSingleton(_embedService)
            .AddSingleton(_commandManager)
            .AddSingleton(_client)
            .AddSingleton(_configService)
            .AddSingleton(_responseParser)
            .AddSingleton(_messageService)
            .AddSingleton(_responseProvider)
            .BuildServiceProvider();
    }
}
