using Discord;
using Discord.WebSocket;

namespace TeaBagBot.Discord.Factories
{
    public static class SocketConfigFactory
    {
        public static DiscordSocketConfig GetDefault()
        {
            return new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info
            };
        }

        public static DiscordSocketConfig GetNew()
        {
            return new DiscordSocketConfig();
        }
    }
}
