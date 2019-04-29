using System;

namespace DiscordBot
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            if (message is null)
                throw new ArgumentException($"{nameof(message)} cannot be null.");
            Console.WriteLine(message);
        }
    }
}
