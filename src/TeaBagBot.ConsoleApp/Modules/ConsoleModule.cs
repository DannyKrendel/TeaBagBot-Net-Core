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
        public async Task Say(string channelName, string message)
        {
            await _messageService.SendMessageAsync(channelName, message);
        }

        [ConsoleCommand("say")]
        public async Task Say(string message)
        {
            var channelId = _configService.LoadConfig().DefaultChannelId;
            await _messageService.SendMessageAsync(channelId, message);
        }

        [ConsoleCommand("restart")]
        public async Task Restart()
        {
            System.Console.WriteLine("restart");
        }

        [ConsoleCommand("exit", aliases: "quit")]
        public async Task Exit()
        {
            System.Console.WriteLine("exit");
        }
    }
}
