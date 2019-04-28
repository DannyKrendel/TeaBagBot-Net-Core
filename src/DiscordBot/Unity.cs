using DiscordBot.Storage.Implementations;
using DiscordBot.Storage.Interfaces;
using DiscordBot.Core;
using Discord.WebSocket;
using Unity;
using Unity.Injection;
using System.IO;
using System.IO.Abstractions;

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
            container.RegisterFactory<DiscordSocketConfig>(x => SocketConfig.GetDefault());
            container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            container.RegisterSingleton<Connection>();
            container.RegisterSingleton<IFileSystem, FileSystem>();
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
