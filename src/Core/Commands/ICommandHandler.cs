using System.Threading.Tasks;

namespace TeaBagBot.Commands
{
    public interface ICommandHandler<T>
    {
        Task InitializeAsync();
        Task HandleCommandAsync(T message);
    }
}