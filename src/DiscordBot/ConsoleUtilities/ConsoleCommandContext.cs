namespace DiscordBot.ConsoleUtilities
{
    public class ConsoleCommandContext
    {
        public string Message { get; }
        public ConsoleCommandHandler Handler { get; }

        public ConsoleCommandContext(string msg, ConsoleCommandHandler handler)
        {
            Message = msg;
            Handler = handler;
        }
    }
}
