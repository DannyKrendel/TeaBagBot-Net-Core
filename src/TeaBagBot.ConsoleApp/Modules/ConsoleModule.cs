using TeaBagBot.ConsoleApp.Commands.Attributes;
using TeaBagBot.Core;
using System;
using System.Threading.Tasks;

namespace TeaBagBot.ConsoleApp.Modules
{
    public class ConsoleModule : ConsoleModuleBase
    {
        private readonly ConfigService _configService;
        private readonly DiscordMessageService _messageService;

        public ConsoleModule(ConfigService configService, DiscordMessageService messageService)
        {
            _configService = configService;
            _messageService = messageService;
        }

        [ConsoleCommand("help")]
        public async Task Help()
        {
            System.Console.WriteLine("help");
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
            System.Console.WriteLine("restart");
        }

        [ConsoleCommand("stop")]
        [ConsoleAlias("exit", "quit")]
        public async Task Exit()
        {
            System.Console.WriteLine("stop");
        }
    }
}
