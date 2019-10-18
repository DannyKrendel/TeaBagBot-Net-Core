using TeaBagBot.Core.Logging.Entities;

namespace TeaBagBot.Core.Logging
{
    public interface ILogger
    {
        void Log(BotLogMessage logMessage);
    }
}
