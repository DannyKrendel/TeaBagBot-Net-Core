using DiscordBot.Core.Logging;
using DiscordBot.Core.Logging.Entities;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class ConsoleHandler
    {
        private readonly ILogger logger;

        public ConsoleHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task CheckMessagesAsync()
        {
            string msg = "";

            await Task.Run(() =>
            {
                do
                {
                    msg = Console.ReadLine();

                    if (msg == "exit")
                        break;

                    HandleMessage(msg);
                } while (true);
            });
        }

        public void HandleMessage(string msg)
        {
            logger.Log(new BotLogMessage(BotLogSeverity.Info, "Console", msg));
        }
    }
}
