using System.Threading.Tasks;

namespace DiscordBot
{
    public interface IBot
    {
        Task StartAsync();
        Task StopAsync();
    }
}