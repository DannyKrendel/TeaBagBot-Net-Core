using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core;
using DiscordBot.Storage.Implementations;
using DiscordBot.Storage.Interfaces;
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
            container.RegisterSingleton<IDataStorage, JsonStorage>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterFactory<DiscordSocketConfig>(x => SocketConfigFactory.GetDefault());
            container.RegisterFactory<CommandServiceConfig>(x => CommandServiceConfigFactory.GetDefault());
            container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            container.RegisterSingleton<Connection>();
            container.RegisterSingleton<DiscordBot>();
            container.RegisterSingleton<IFileSystem, FileSystem>();
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
