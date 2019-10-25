using TeaBagBot.ConsoleApp.Commands.Attributes;
using TeaBagBot.Core;
using System;
using System.Threading.Tasks;
using TeaBagBot.Discord;

namespace TeaBagBot.ConsoleApp.CommandModules
{
    public class ConsoleModule : ConsoleModuleBase
    {
        private readonly ConfigService _configService;
        private readonly DiscordMessageService _messageService;
        private readonly IBot _bot;

        public ConsoleModule(ConfigService configService, DiscordMessageService messageService, IBot bot)
        {
            _configService = configService;
            _messageService = messageService;
            _bot = bot;
        }

        [ConsoleCommand("help")]
        public async Task Help()
        {
            Console.WriteLine("help");
        }

        [ConsoleCommand("say")]
        [ConsoleAlias("echo")]
        public async Task Say(string message, string channel = null)
        {
            if (channel == null)
            {
                var channelId = _configService.LoadConfig().DefaultChannelId;
                await _messageService.SendMessageAsync(channelId, message);
            }
            else
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
        }

        [ConsoleCommand("restart")]
        [ConsoleAlias("reload")]
        public async Task Restart()
        {
            await _bot.StopAsync().ContinueWith(x => _bot.StartAsync());
        }

        [ConsoleCommand("start")]
        public async Task Start()
        {
            await _bot.StartAsync();
        }

        [ConsoleCommand("stop")]
        [ConsoleAlias("exit", "quit")]
        public async Task Stop()
        {
            await _bot.StopAsync();
        }
    }
}
