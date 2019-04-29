using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        private static async Task Main()
        {
            var bot = Unity.Resolve<DiscordBot>();
            await bot.Start();
            await Task.Delay(-1);
        }
    }
}
