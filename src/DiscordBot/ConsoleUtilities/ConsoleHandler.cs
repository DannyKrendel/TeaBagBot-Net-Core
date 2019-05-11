using DiscordBot.Core.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.ConsoleUtilities
{
    public class ConsoleHandler
    {
        private readonly ILogger logger;

        public ConsoleHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<(ConsoleCommand?, string)> CheckMessagesAsync()
        {
            string msg = "";

            return await Task.Run(() =>
            {
                msg = Console.ReadLine();
                return HandleMessage(msg);
            });
        }

        public (ConsoleCommand?, string) HandleMessage(string msg)
        {
            var commands = Enum.GetValues(typeof(ConsoleCommand)).OfType<ConsoleCommand>();
            string command = msg.Split(new char[] { ' ' })[0].ToLower();

            if (!commands.Select(n => n.ToString().ToLower()).Contains(command))
                return (null, msg);

            return (commands.First(n => n.ToString().ToLower() == command.ToLower()), "");
        }
    }
}
