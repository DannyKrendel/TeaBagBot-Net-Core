using Discord.Commands;
using Discord.WebSocket;
using TeaBagBot.Core.Commands;
using TeaBagBot.Core.Entities;
using TeaBagBot.Core.Factories;
using TeaBagBot.Core.Logging;
using TeaBagBot.Core.Storage;
using TeaBagBot.Core.Storage.Json;
using TeaBagBot.Core.Storage.Memory;
using System;
using System.IO.Abstractions;
using Unity;
using Unity.Injection;
using TeaBagBot.Core.Messages;

namespace TeaBagBot.Core.DI
{
    public static class UnityContainerExtensions
    {
        public static void RegisterCoreTypes(this UnityContainer container)
        {
            if (container == null)
                return;

            container.RegisterSingleton<IFileSystem, FileSystem>();
            container.RegisterSingleton<IDataStorage, MemoryStorage>();
            container.RegisterSingleton<MemoryStorage>();
            container.RegisterSingleton<JsonStorage>();
            container.RegisterSingleton<TokenService>();
            container.RegisterSingleton<DataStorageService>();
            container.RegisterSingleton<DiscordLogger>();
            container.RegisterSingleton<ConfigService>();
            container.RegisterFactory<TeaBagConfig>(x => container.Resolve<ConfigService>().LoadConfig());
            container.RegisterFactory<DiscordSocketConfig>(x => SocketConfigFactory.GetDefault());
            container.RegisterFactory<CommandServiceConfig>(x => CommandServiceConfigFactory.GetDefault());
            container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            container.RegisterSingleton<CommandService>(new InjectionConstructor(typeof(CommandServiceConfig)));

            container.RegisterSingleton<IBot, TeaBagBot>();
            container.RegisterSingleton<ServiceBuilder>();
            container.RegisterSingleton<EmbedService>();
            container.RegisterSingleton<TeaBagCommandProvider>();
            container.RegisterSingleton<TeaBagCommandService>();
            container.RegisterSingleton<Connection>();
            container.RegisterFactory<IServiceProvider>(x => container.Resolve<ServiceBuilder>().BuildServices());
            container.RegisterSingleton<ICommandHandler<SocketMessage>, CommandHandler>();
            container.RegisterSingleton<ResponseParser>();
            container.RegisterSingleton<DiscordMessageService>();
            container.RegisterSingleton<ResponseService>();
            container.RegisterSingleton<ResponseProvider>();
            container.RegisterSingleton<IMessageHandler<SocketMessage>, MessageHandler>();
        }
    }
}
