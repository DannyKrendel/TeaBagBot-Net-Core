using DiscordBot.ConsoleUtils.Attributes;
using DiscordBot.ConsoleUtils.Modules;
using DiscordBot.Core;
using System;
using System.Threading.Tasks;

namespace DiscordBot.ConsoleUtils.Modules
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
            Console.WriteLine("help");
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
            Console.WriteLine("restart");
        }

        [ConsoleCommand("exit", aliases: "quit")]
        public async Task Exit()
        {
            Console.WriteLine("exit");
        }
    }
}
