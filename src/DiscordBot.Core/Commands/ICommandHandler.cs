using System.Threading.Tasks;
using Discord.WebSocket;

namespace TeaBagBot.Core.Commands
{
    public interface ICommandHandler<T>
    {
        Task HandleMessageAsync(T msg);
        Task InitializeAsync();
    }
}