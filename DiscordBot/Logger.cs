using System;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
