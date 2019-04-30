using System.Threading.Tasks;

namespace DiscordBot.Extensions
{
    class Program
    {
        private static async Task Main()
        {
            var bot = Unity.Resolve<DiscordBot>();
            await bot.StartAsync();
            await Task.Delay(-1);
        }
    }
}
