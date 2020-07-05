using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;
using TeaBagBot.Messages;
using TeaBagBot.Services;

namespace TeaBagBot.DI
{
    public class ServiceBuilder
    {
        private readonly IRepository<BotConfig> _configRepository;
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly TeaBagCommandService _commandService;
        private readonly ResponseParser _responseParser;
        private readonly ResponseService _responseService;
        private readonly GamesListService _gamesListService;
        private readonly LinkService _linkService;

        public ServiceBuilder(IRepository<BotConfig> configRepository, 
            DiscordSocketClient client, EmbedService embedService,
            TeaBagCommandService commandService, ResponseParser responseParser,
            ResponseService responseService, GamesListService gamesListService,
            LinkService linkService)
        {
            _configRepository = configRepository;
            _client = client;
            _embedService = embedService;
            _commandService = commandService;
            _responseParser = responseParser;
            _responseService = responseService;
            _gamesListService = gamesListService;
            _linkService = linkService;
        }

        public IServiceProvider BuildServices() => new ServiceCollection()
            .AddSingleton(_embedService)
            .AddSingleton(_commandService)
            .AddSingleton(_client)
            .AddSingleton(_responseParser)
            .AddSingleton(_responseService)
            .AddSingleton(_gamesListService)
            .AddSingleton(_configRepository)
            .AddSingleton(_linkService)
            .BuildServiceProvider();
    }
}
