using System.Threading.Tasks;
using Discord;

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
            logger.Log(logMsg.Message);
            return Task.CompletedTask;
        }
    }
}
