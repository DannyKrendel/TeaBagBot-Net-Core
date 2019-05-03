using System;

namespace DiscordBot
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            if (message is null)
                Console.WriteLine("Something went wrong and message was null.");
            else
                Console.WriteLine(message);
        }

        public void LogException(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
