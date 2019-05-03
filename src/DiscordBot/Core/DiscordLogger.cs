using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    public class DiscordLogger
    {
        private readonly ILogger logger;

        public DiscordLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task Log(LogMessage logMsg)
        {
            logger.Log(logMsg.ToString());

            await Task.CompletedTask;
        }

        public async Task LogException(string source, Exception ex)
        {
            await Log(new LogMessage(LogSeverity.Error, source, null, ex));
        }

        public async Task LogCommandResult(IResult result, SocketCommandContext context)
        {
            if (result.Error is null)
                return;

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
                    reason = result.ErrorReason;
                    break;
                case CommandError.Unsuccessful:
                    reason = result.ErrorReason;
                    break;
            }

            await context.Channel.SendMessageAsync(reason);
        }
    }
}
