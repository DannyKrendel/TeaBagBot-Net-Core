using TeaBagBot.ConsoleApp.Commands.Attributes;
using System;
using System.Threading.Tasks;
using TeaBagBot.Services;

namespace TeaBagBot.ConsoleApp.Modules
{
    public class ConsoleModule : ConsoleModuleBase
    {
        private readonly DiscordMessageService _messageService;
        private readonly IBot _bot;

        public ConsoleModule(DiscordMessageService messageService, IBot bot)
        {
            _messageService = messageService;
            _bot = bot;
        }

        [ConsoleCommand("help", true)]
        public async Task Help()
        {
            Console.WriteLine("help");
        }

        [ConsoleCommand("say", false)]
        [ConsoleAlias("echo")]
        public async Task Say(string message, string channel)
        {
            if (ulong.TryParse(channel, out ulong channelId))
            {
                await _messageService.SendMessageAsync(channelId, message);
            }
            else
            {
                await _messageService.SendMessageAsync(channel, message);
            }
        }

        [ConsoleCommand("restart", true)]
        [ConsoleAlias("reload")]
        public async Task Restart()
        {
            await _bot.StopAsync().ContinueWith(x => _bot.StartAsync());
        }

        [ConsoleCommand("start", true)]
        public async Task Start()
        {
            await _bot.StartAsync();
        }

        [ConsoleCommand("stop", true)]
        [ConsoleAlias("exit", "quit")]
        public async Task Stop()
        {
            await _bot.StopAsync();
        }
    }
}
