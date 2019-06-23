namespace DiscordBot.ConsoleUtils
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
