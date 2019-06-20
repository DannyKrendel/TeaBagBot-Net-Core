using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiscordBot.Core
{
    public class CoreServiceInitializer
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly CommandManager _commandManager;
        private readonly ConfigService _configService;
        private readonly CommandParser _commandParser;

        public CoreServiceInitializer(DiscordSocketClient client, EmbedService embedService,
            CommandManager commandManager, ConfigService config, CommandParser commandParser)
        {
            _client = client;
            _embedService = embedService;
            _commandManager = commandManager;
            _configService = config;
            _commandParser = commandParser;
        }

        public IServiceProvider BuildServices() => new ServiceCollection()
            .AddSingleton(_embedService)
            .AddSingleton(_commandManager)
            .AddSingleton(_client)
            .AddSingleton(_configService)
            .AddSingleton(_commandParser)
            .BuildServiceProvider();
    }
}
