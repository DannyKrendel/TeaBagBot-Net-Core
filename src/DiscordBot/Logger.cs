using System;

namespace DiscordBot
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message), "Message was null.");
            if (message == "")
                throw new ArgumentException(nameof(message), "Message was empty.");

            Console.WriteLine(message);
        }
    }
}
