using Discord.Commands;
using Discord.WebSocket;
using TeaBagBot.Commands;
using TeaBagBot.Models;
using System;
using System.IO.Abstractions;
using Unity;
using Unity.Injection;
using TeaBagBot.Messages;
using TeaBagBot.Services;
using Discord;
using TeaBagBot.DataAccess;

namespace TeaBagBot.DI
{
    public static class UnityContainerExtensions
    {
        public static void RegisterCoreTypes(this UnityContainer container)
        {
            if (container == null)
                return;

            container.RegisterSingleton<IFileSystem, FileSystem>();
            container.RegisterSingleton<SettingsService>();
            container.RegisterFactory<AppSettings>(x => container.Resolve<SettingsService>().Load());
            container.RegisterFactory<DiscordSocketConfig>(x => new DiscordSocketConfig() 
            { LogLevel = LogSeverity.Info });
            container.RegisterFactory<CommandServiceConfig>(x => new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose
            });
            container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            container.RegisterSingleton<CommandService>(new InjectionConstructor(typeof(CommandServiceConfig)));

            container.RegisterSingleton<IBot, TeaBagBot>();
            container.RegisterSingleton<ServiceBuilder>();
            container.RegisterSingleton<EmbedService>();
            container.RegisterSingleton<TeaBagCommandService>();
            container.RegisterSingleton<Connection>();
            container.RegisterFactory<IServiceProvider>(x => container.Resolve<ServiceBuilder>().BuildServices());
            container.RegisterSingleton<ICommandHandler<SocketMessage>, CommandHandler>();
            container.RegisterSingleton<ResponseParser>();
            container.RegisterSingleton<DiscordMessageService>();
            container.RegisterSingleton<ResponseService>();
            container.RegisterSingleton<IMessageHandler<SocketMessage>, MessageHandler>();
            container.RegisterSingleton<LoggingService>();
            container.RegisterSingleton<GoogleSheetsService>();
            container.RegisterSingleton<GamesListService>();
            container.RegisterFactory<IMongoDbSettings>(x => container.Resolve<SettingsService>().Load().MongoDbSettings);
            container.RegisterType(typeof(IRepository<>), typeof(MongoRepository<>));
            container.RegisterSingleton<LinkService>();
        }
    }
}
