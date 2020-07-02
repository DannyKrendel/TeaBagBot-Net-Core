using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TeaBagBot.Services;

namespace TeaBagBot.ConsoleApp.DI
{
    public class ConsoleServiceBuilder
    {
        private readonly DiscordMessageService _messageService;
        private readonly IBot _bot;

        public ConsoleServiceBuilder(DiscordMessageService messageService, IBot bot)
        {
            _messageService = messageService;
            _bot = bot;
        }

        public IServiceProvider BuildServices() => new ServiceCollection()
            .AddSingleton(_messageService)
            .AddSingleton(_bot)
            .BuildServiceProvider();
    }
}
