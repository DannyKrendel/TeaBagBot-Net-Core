using DiscordBot.Core.Logging;
using DiscordBot.Core.Logging.Entities;
using System;

namespace DiscordBot.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(BotLogMessage logMessage)
        {
            ConsoleColor color = Console.ForegroundColor;

            switch (logMessage.Severity)
            {
                case BotLogSeverity.Error:
                    color = ConsoleColor.Red;
                    break;
                case BotLogSeverity.Warning:
                    color = ConsoleColor.Yellow;
                    break;
                case BotLogSeverity.Info:
                    color = ConsoleColor.Gray;
                    break;
            }

            LogWithColor(logMessage.ToString(), color);
        }

        private void LogWithColor(string message, ConsoleColor color)
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prevColor;
        }
    }
}
