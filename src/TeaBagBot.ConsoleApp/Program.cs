using System;
using System.Threading.Tasks;
using TeaBagBot.ConsoleApp.DI;
using TeaBagBot.Core;
using TeaBagBot.Core.Commands;
using TeaBagBot.Core.Storage;
using Unity.Resolution;

namespace TeaBagBot.ConsoleApp
{
    class Program
    {
        private static async Task Main()
        {
            UnityDI.Resolve<DataStorageService>().LoadEverythingToMemory();
            var bot = UnityDI.Resolve<IBot>();
            await bot.StartAsync();
            await UnityDI.Resolve<ICommandHandler<string>>(
                new ParameterOverride(typeof(IServiceProvider), UnityDI.Resolve<IServiceProvider>("console")))
                .InitializeAsync();
        }
    }
}
