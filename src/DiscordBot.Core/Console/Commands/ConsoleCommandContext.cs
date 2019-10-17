namespace DiscordBot.Console.Commands
{
    public class ConsoleCommandContext
    {
        public string Message { get; }

        public ConsoleCommandContext(string msg)
        {
            Message = msg;
        }
    }
}
