using DiscordBot.ConsoleUtilities.Attributes;
using System;
using System.Threading.Tasks;

namespace DiscordBot.ConsoleUtilities
{
    public class ConsoleCommands
    {
        public ConsoleCommandContext Context { get; set; }

        public ConsoleCommands()
        {

        }

        [ConsoleCommand("help")]
        public async Task Help()
        {
            Console.WriteLine("help");
        }

        [ConsoleCommand("say")]
        public async Task Say(string text)
        {
            Console.WriteLine("say");
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
