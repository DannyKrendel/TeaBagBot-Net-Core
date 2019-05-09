using Discord.WebSocket;
using DiscordBot.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiscordBot.Core
{
    public class ServiceProviderInitializer
    {
        private readonly DiscordSocketClient client;
        private readonly EmbedService embedService;
        private readonly CommandManager commandManager;
        private readonly ConfigService configService;

        public ServiceProviderInitializer(DiscordSocketClient client, EmbedService embedService, CommandManager commandManager, ConfigService config)
        {
            this.client = client;
            this.embedService = embedService;
            this.commandManager = commandManager;
            this.configService = config;
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(embedService)
            .AddSingleton(commandManager)
            .AddSingleton(client)
            .AddSingleton(configService)
            .BuildServiceProvider();
    }
}
