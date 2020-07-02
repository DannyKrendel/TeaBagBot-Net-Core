using System;
using System.Threading.Tasks;
using TeaBagBot.ConsoleApp.DI;

namespace TeaBagBot.ConsoleApp
{
    class Program
    {
        private static async Task Main()
        {
            await UnityDI.Resolve<Startup>().InitializeAsync();
        }
    }
}
