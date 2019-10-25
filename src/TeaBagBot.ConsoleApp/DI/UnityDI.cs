using TeaBagBot.ConsoleApp.Commands;
using TeaBagBot.Core.Logging;
using Unity;
using TeaBagBot.ConsoleApp.Logging;
using TeaBagBot.Core.Commands;
using TeaBagBot.Discord.DI;
using Unity.Injection;
using System;
using Unity.Resolution;

namespace TeaBagBot.ConsoleApp.DI
{
    public static class UnityDI
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

            _container.RegisterCoreTypes();
            _container.RegisterSingleton<ILogger, ConsoleLogger>();
            _container.RegisterSingleton<ConsoleCommandParser>();
            _container.RegisterSingleton<ConsoleCommandService>();
            _container.RegisterSingleton<ConsoleCommandBuilder>();
            _container.RegisterFactory<IServiceProvider>("console", x => Resolve<ConsoleServiceBuilder>().BuildServices());
            _container.RegisterSingleton<ICommandHandler<string>, ConsoleCommandHandler>();

            _container.AddExtension(new Diagnostic());
        }

        public static T Resolve<T>(string name, params ResolverOverride[] overrides)
        {
            return Container.Resolve<T>(name, overrides);
        }

        public static T Resolve<T>(params ResolverOverride[] overrides)
        {
            return Container.Resolve<T>(overrides);
        }
    }
}
