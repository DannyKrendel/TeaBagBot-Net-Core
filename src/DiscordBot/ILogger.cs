using System;

namespace DiscordBot
{
    public interface ILogger
    {
        void Log(string message);
        void LogException(Exception ex);
    }
}
