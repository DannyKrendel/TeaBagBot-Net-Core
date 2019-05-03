using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiscordBot.Core
{
    public class ServiceProviderInitializer
    {
        private readonly EmbedService embedService;
        private readonly CommandManager commandManager;

        public ServiceProviderInitializer(EmbedService embedService, CommandManager commandManager)
        {
            this.embedService = embedService;
            this.commandManager = commandManager;
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(embedService)
            .AddSingleton(commandManager)
            .BuildServiceProvider();
    }
}
