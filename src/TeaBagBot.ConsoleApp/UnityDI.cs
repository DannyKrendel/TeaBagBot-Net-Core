using TeaBagBot.ConsoleApp;
using TeaBagBot.ConsoleApp.Commands;
using TeaBagBot.Core.Logging;
using Unity;
using Unity.Injection;
using TeaBagBot.Core.Helpers;
using TeaBagBot.ConsoleApp.Logging;
using TeaBagBot.Core.Commands;
using TeaBagBot.Core.DI;

namespace TeaBagBot.ConsoleApp
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
            _container.RegisterSingleton<ConsoleCommandBuilder>();
            _container.RegisterSingleton<ConsoleCommandService>();
            _container.RegisterSingleton<ICommandHandler<string>, ConsoleCommandHandler>();

            _container.AddExtension(new Diagnostic());
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
