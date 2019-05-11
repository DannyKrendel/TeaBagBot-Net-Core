using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.ConsoleUtilities;
using DiscordBot.Core;
using DiscordBot.Core.Entities;
using DiscordBot.Core.Factories;
using DiscordBot.Core.Logging;
using DiscordBot.Logging;
using DiscordBot.Storage.Implementations;
using DiscordBot.Storage.Interfaces;
using System;
using System.IO.Abstractions;
using Unity;
using Unity.Injection;

namespace DiscordBot
{
    public static class Unity
    {
        private static UnityContainer container;

        public static UnityContainer Container
        {
            get
            {
                if (container == null)
                    RegisterTypes();
                return container;
            }
        }

        public static void RegisterTypes()
        {
            container = new UnityContainer();
            container.RegisterSingleton<IFileSystem, FileSystem>();
            container.RegisterSingleton<IDataStorage, MemoryStorage>();
            container.RegisterSingleton<MemoryStorage>();
            container.RegisterSingleton<JsonStorage>();
            container.RegisterSingleton<DataStorageService>();

            container.RegisterSingleton<ILogger, ConsoleLogger>();
            container.RegisterSingleton<DiscordLogger>();

            container.RegisterSingleton<ConfigService>();
            container.RegisterFactory<BotConfig>(x => container.Resolve<ConfigService>().LoadConfig());
            container.RegisterFactory<DiscordSocketConfig>(x => SocketConfigFactory.GetDefault());
            container.RegisterFactory<CommandServiceConfig>(x => CommandServiceConfigFactory.GetDefault());
            container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            container.RegisterSingleton<CommandService>(new InjectionConstructor(typeof(CommandServiceConfig)));

            container.RegisterSingleton<ServiceProviderInitializer>();
            container.RegisterSingleton<EmbedService>();
            container.RegisterFactory<IServiceProvider>(x => container.Resolve<ServiceProviderInitializer>().BuildServiceProvider());
            container.RegisterSingleton<CommandManager>();
            container.RegisterSingleton<CommandEntityService>();
            container.RegisterSingleton<CommandHandler>();
            container.RegisterSingleton<Connection>();
            container.RegisterSingleton<DiscordBot>();
            container.RegisterSingleton<ConsoleHandler>();
            container.RegisterSingleton<CommandParser>();

            container.AddExtension(new Diagnostic());
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
