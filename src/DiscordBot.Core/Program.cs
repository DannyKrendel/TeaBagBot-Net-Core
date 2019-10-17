using DiscordBot.Storage;
using System.Threading.Tasks;

namespace DiscordBot.Extensions
{
    class Program
    {
        private static async Task Main()
        {
            Unity.Resolve<DataStorageService>().LoadEverythingToMemory();
            var bot = Unity.Resolve<IBot>();
            await bot.StartAsync();
        }
    }
}
