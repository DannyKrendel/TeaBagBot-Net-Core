using System;

namespace DiscordBot
{
    public class Logger : ILogger
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
            while (ex != null)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                ex = ex.InnerException;
            }
        }
    }
}
