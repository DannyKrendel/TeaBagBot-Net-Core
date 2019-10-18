using Discord;
using Discord.WebSocket;

namespace TeaBagBot.Core.Factories
{
    public static class SocketConfigFactory
    {
        public static DiscordSocketConfig GetDefault()
        {
            return new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug
            };
        }

        public static DiscordSocketConfig GetNew()
        {
            return new DiscordSocketConfig();
        }
    }
}
