using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TeaBagBot.Core;
using TeaBagBot.Discord;

namespace TeaBagBot.ConsoleApp.DI
{
    public class ConsoleServiceBuilder
    {
        private readonly ConfigService _configService;
        private readonly DiscordMessageService _messageService;
        private readonly IBot _bot;

        public ConsoleServiceBuilder(ConfigService configService, DiscordMessageService messageService, IBot bot)
        {
            _configService = configService;
            _messageService = messageService;
            _bot = bot;
        }

        public IServiceProvider BuildServices() => new ServiceCollection()
            .AddSingleton(_configService)
            .AddSingleton(_messageService)
            .AddSingleton(_bot)
            .BuildServiceProvider();
    }
}
