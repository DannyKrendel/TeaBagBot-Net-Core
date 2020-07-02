using System.Threading.Tasks;

namespace TeaBagBot
{
    public interface IBot
    {
        Task StartAsync();
        Task StopAsync();
    }
}