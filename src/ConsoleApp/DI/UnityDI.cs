using TeaBagBot.ConsoleApp.Commands;
using Unity;
using TeaBagBot.Commands;
using TeaBagBot.DI;
using Unity.Injection;
using System;
using Unity.Resolution;
using TeaBagBot.ConsoleApp.Services;
using Serilog;
using Microsoft.Extensions.Configuration;
using TeaBagBot.Services;
using System.Security.Cryptography.X509Certificates;

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

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Resolve<SettingsService>().LoadConfiguration(), "Serilog")
                .CreateLogger();

            _container.RegisterInstance<ILogger>(Log.Logger);
            _container.RegisterSingleton<ConsoleCommandParser>();
            _container.RegisterSingleton<ConsoleCommandService>();
            _container.RegisterSingleton<ConsoleCommandBuilder>();
            _container.RegisterFactory<IConfiguration>(x => Resolve<SettingsService>().LoadConfiguration());
            _container.RegisterSingleton<ICommandHandler<string>, ConsoleCommandHandler>(
                new InjectionConstructor(typeof(ILogger), typeof(ConsoleCommandService), Resolve<ConsoleServiceBuilder>().BuildServices()));
            _container.RegisterSingleton<Startup>();

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
