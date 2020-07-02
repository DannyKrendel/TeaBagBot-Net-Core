using TeaBagBot.ConsoleApp.Commands;

namespace TeaBagBot.ConsoleApp.Modules
{
    public abstract class ConsoleModuleBase : ConsoleModuleBase<IConsoleCommandContext> { }

    public abstract class ConsoleModuleBase<T> where T : class
    {
        public T Context { get; set; }
    }
}
