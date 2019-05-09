using Discord;
using Discord.Commands;
using DiscordBot.Core.Logging.Entities;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Core.Logging
{
    public class DiscordLogger
    {
        private readonly ILogger logger;

        public DiscordLogger(ILogger logger)
        {
            this.logger = logger;
        }

        internal async Task LogAsync(LogMessage logMsg)
        {
            logger.Log(LogMessageConverter.ToBotLogMessage(logMsg));
            await Task.CompletedTask;
        }

        public async Task LogErrorAsync(string source, Exception exception)
        {
            logger.Log(new BotLogMessage(BotLogSeverity.Error, source, null, exception));
            await Task.CompletedTask;
        }

        public async Task LogWarningAsync(string source, string message)
        {
            logger.Log(new BotLogMessage(BotLogSeverity.Warning, source, message));
            await Task.CompletedTask;
        }

        public async Task LogCommandErrorAsync(IResult result, ICommandContext context)
        {
            string reason = "";

            switch (result.Error)
            {
                case CommandError.UnknownCommand:
                    reason = $"{context.User.Mention}, вы ввели неизвестную команду.";
                    break;
                case CommandError.ParseFailed:
                    reason = $"{context.User.Mention}, команду невозможно обработать.";
                    break;
                case CommandError.BadArgCount:
                    reason = $"{context.User.Mention}, неверное количество аргументов.";
                    break;
                case CommandError.ObjectNotFound:
                    reason = result.ErrorReason;
                    break;
                case CommandError.MultipleMatches:
                    reason = result.ErrorReason;
                    break;
                case CommandError.UnmetPrecondition:
                    reason = result.ErrorReason;
                    break;
                case CommandError.Exception:
                    reason = $"Возникло исключение: {result.ErrorReason}";
                    break;
                case CommandError.Unsuccessful:
                    reason = result.ErrorReason;
                    break;
            }

            await context.Channel.SendMessageAsync(reason);
            await LogWarningAsync("Command", reason);
        }

        public async Task LogCommandResultAsync(Optional<CommandInfo> command, ICommandContext context)
        {
            var commandName = command.IsSpecified ? command.Value.Name : "Unknown command";
            var user = context.User;

            await LogAsync(new LogMessage(
                LogSeverity.Info,
                "Command",
                $"{commandName} was executed by {user}."));
        }
    }
}
