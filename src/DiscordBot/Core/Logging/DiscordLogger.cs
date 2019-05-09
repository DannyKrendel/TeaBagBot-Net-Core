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
        private readonly EmbedService embedService;

        public DiscordLogger(ILogger logger, EmbedService embedService)
        {
            this.logger = logger;
            this.embedService = embedService;
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

        public async Task LogInfoAsync(string source, string message)
        {
            logger.Log(new BotLogMessage(BotLogSeverity.Info, source, message));
            await Task.CompletedTask;
        }

        public async Task LogCommandResultAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result.Error.HasValue)
            {
                string reason = "";
                string log = "";

                switch (result.Error)
                {
                    case CommandError.UnknownCommand:
                        reason = $"{context.User.Mention}, вы ввели неизвестную команду.";
                        log = $"Неизвестная команда.";
                        break;
                    case CommandError.ParseFailed:
                        reason = $"{context.User.Mention}, команду невозможно обработать.";
                        log = $"Команду невозможно обработать.";
                        break;
                    case CommandError.BadArgCount:
                        reason = $"{context.User.Mention}, неверное количество аргументов.\n" +
                            $"Напишите `help {command.Value.Name}`, чтобы посмотреть использование команды.";
                        log = $"Неверное количество аргументов.";
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
                        reason = $"При выполнении команды возникла ошибка.";
                        log = $"Возникло исключение: {result.ErrorReason}";
                        break;
                    case CommandError.Unsuccessful:
                        reason = result.ErrorReason;
                        break;
                }

                var embed = embedService.GetErrorEmbed("Ошибка!", reason);
                await context.Channel.SendMessageAsync(embed: embed);
                await LogWarningAsync("Command", log); 
            }

            var commandName = command.IsSpecified ? command.Value.Name : "Неизвестная команда";
            var user = context.User;

            await LogInfoAsync("Command", $"{commandName} была вызвана {user}.");
        }
    }
}
