using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Console;
using DiscordBot.Console.Commands;
using DiscordBot.Core;
using DiscordBot.Core.Entities;
using DiscordBot.Core.Factories;
using DiscordBot.Core.Logging;
using DiscordBot.Logging;
using DiscordBot.Storage;
using DiscordBot.Storage.Json;
using DiscordBot.Storage.Memory;
using System;
using System.IO.Abstractions;
using Unity;
using Unity.Injection;

namespace DiscordBot
{
    public static class Unity
    {
        private static UnityContainer _container;

        public static UnityContainer Container
        {
            get
            {
                if (_container == null)
                    RegisterTypes();
                return _container;
            }
        }

        public static void RegisterTypes()
        {
            _container = new UnityContainer();
            _container.RegisterSingleton<IFileSystem, FileSystem>();
            _container.RegisterSingleton<IDataStorage, MemoryStorage>();
            _container.RegisterSingleton<MemoryStorage>();
            _container.RegisterSingleton<JsonStorage>();
            _container.RegisterSingleton<DataStorageService>();

            _container.RegisterSingleton<ILogger, ConsoleLogger>();
            _container.RegisterSingleton<DiscordLogger>();

            _container.RegisterSingleton<ConfigService>();
            _container.RegisterFactory<BotConfig>(x => Resolve<ConfigService>().LoadConfig());
            _container.RegisterFactory<DiscordSocketConfig>(x => SocketConfigFactory.GetDefault());
            _container.RegisterFactory<CommandServiceConfig>(x => CommandServiceConfigFactory.GetDefault());
            _container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            _container.RegisterSingleton<CommandService>(new InjectionConstructor(typeof(CommandServiceConfig)));

            _container.RegisterSingleton<IBot, DiscordBot>();
            _container.RegisterSingleton<CoreServiceInitializer>();
            _container.RegisterSingleton<EmbedService>();
            _container.RegisterFactory<IServiceProvider>(x => Resolve<CoreServiceInitializer>().BuildServices());
            _container.RegisterSingleton<CommandManager>();
            _container.RegisterSingleton<CommandEntityService>();
            _container.RegisterSingleton<Connection>();
            _container.RegisterSingleton<ICommandHandler<SocketMessage>, CommandHandler>();
            _container.RegisterSingleton<ConsoleCommandBuilder>();
            _container.RegisterSingleton<ConsoleCommandService>();
            _container.RegisterSingleton<ICommandHandler<string>, ConsoleCommandHandler>();
            _container.RegisterSingleton<CommandParser>();
            _container.RegisterSingleton<DiscordMessageService>();

            _container.AddExtension(new Diagnostic());
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
