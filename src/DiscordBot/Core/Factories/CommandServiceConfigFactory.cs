using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Core
{
    public static class CommandServiceConfigFactory
    {
        public static CommandServiceConfig GetDefault()
        {
            return new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose
            };
        }

        public static CommandServiceConfig GetNew()
        {
            return new CommandServiceConfig();
        }
    }
}
