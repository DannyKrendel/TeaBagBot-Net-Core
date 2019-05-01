using Discord;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    public class DiscordLogger
    {
        private ILogger logger;

        public DiscordLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public Task Log(LogMessage logMsg)
        {
            logger.Log(logMsg.ToString());

            if (logMsg.Exception != null)
                logger.LogException(logMsg.Exception);

            return Task.CompletedTask;
        }
    }
}
