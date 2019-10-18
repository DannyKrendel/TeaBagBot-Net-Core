using System.Threading.Tasks;

namespace TeaBagBot.Core
{
    public interface IBot
    {
        Task StartAsync();
        Task StopAsync();
    }
}