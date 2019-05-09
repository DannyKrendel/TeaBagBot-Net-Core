using DiscordBot.Core.Logging.Entities;

namespace DiscordBot.Core.Logging
{
    public interface ILogger
    {
        void Log(BotLogMessage logMessage);
    }
}
