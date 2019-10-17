using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordBot.Commands
{
    public interface ICommandHandler<T>
    {
        Task HandleMessageAsync(T msg);
        Task InitializeAsync();
    }
}