using System.Threading.Tasks;

namespace TeaBagBot.Core.Commands
{
    public interface ICommandHandler<T>
    {
        Task HandleCommandAsync(T message);
        Task InitializeAsync();
    }
}