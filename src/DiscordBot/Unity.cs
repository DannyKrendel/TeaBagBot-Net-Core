using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.ConsoleUtils;
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
            _container.RegisterFactory<BotConfig>(x => _container.Resolve<ConfigService>().LoadConfig());
            _container.RegisterFactory<DiscordSocketConfig>(x => SocketConfigFactory.GetDefault());
            _container.RegisterFactory<CommandServiceConfig>(x => CommandServiceConfigFactory.GetDefault());
            _container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            _container.RegisterSingleton<CommandService>(new InjectionConstructor(typeof(CommandServiceConfig)));

            _container.RegisterSingleton<DiscordBot>();
            _container.RegisterSingleton<CoreServiceInitializer>();
            _container.RegisterSingleton<EmbedService>();
            _container.RegisterFactory<IServiceProvider>(x => _container.Resolve<CoreServiceInitializer>().BuildServices());
            _container.RegisterSingleton<CommandManager>();
            _container.RegisterSingleton<CommandEntityService>();
            _container.RegisterSingleton<CommandHandler>();
            _container.RegisterSingleton<Connection>();
            _container.RegisterSingleton<ConsoleCommandBuilder>();
            _container.RegisterSingleton<ConsoleCommandService>();
            _container.RegisterSingleton<ConsoleCommandHandler>();
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
