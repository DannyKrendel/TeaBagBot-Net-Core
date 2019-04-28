using Discord;
using Discord.WebSocket;

namespace DiscordBot.Core
{
    public static class SocketConfig
    {
        public static DiscordSocketConfig GetDefault()
        {
            return new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose
            };
        }

        public static DiscordSocketConfig GetNew()
        {
            return new DiscordSocketConfig();
        }
    }
}
