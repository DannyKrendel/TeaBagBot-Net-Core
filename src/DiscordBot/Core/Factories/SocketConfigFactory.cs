using Discord;
using Discord.WebSocket;

namespace DiscordBot.Core.Factories
{
    public static class SocketConfigFactory
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
