using System;
using System.Threading.Tasks;
using TeaBagBot.Core;
using TeaBagBot.Core.Storage;

namespace TeaBagBot.ConsoleApp
{
    class Program
    {
        private static async Task Main()
        {
            UnityDI.Resolve<DataStorageService>().LoadEverythingToMemory();
            var bot = UnityDI.Resolve<IBot>();
            await bot.StartAsync();
        }
    }
}
