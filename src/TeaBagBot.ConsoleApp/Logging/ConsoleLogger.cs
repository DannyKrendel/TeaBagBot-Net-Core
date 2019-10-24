using System;
using TeaBagBot.Core.Logging;

namespace TeaBagBot.ConsoleApp.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(BotLogMessage logMessage)
        {
            ConsoleColor color = System.Console.ForegroundColor;

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
            ConsoleColor prevColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = prevColor;
        }
    }
}
