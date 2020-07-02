namespace TeaBagBot.ConsoleApp.Commands
{
    public class ConsoleCommandContext : IConsoleCommandContext
    {
        public string Message { get; }

        public ConsoleCommandContext(string msg)
        {
            Message = msg;
        }
    }
}
