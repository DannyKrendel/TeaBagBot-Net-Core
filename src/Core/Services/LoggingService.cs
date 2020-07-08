using Discord;
using Discord.Commands;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TeaBagBot.Services
{
    public class LoggingService
    {
        private readonly ILogger _logger;
        private readonly EmbedService _embedService;

        public LoggingService(ILogger logger, EmbedService embedService)
        {
            _logger = logger;
            _embedService = embedService;
        }

        public async Task LogCommandResultAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result.Error.HasValue)
            {
                string reason = "";

                switch (result.Error)
                {
                    case CommandError.UnknownCommand:
                        reason = $"{context.User.Mention}, вы ввели неизвестную команду.";
                        _logger.Warning("Введена неизвестная команда: {message}. Пользователь: {user}", 
                            context.Message, context.User);
                        break;
                    case CommandError.ParseFailed:
                        reason = $"{context.User.Mention}, команду невозможно обработать.";
                        _logger.Warning("Невозможно обработать команду \"{command}\". Пользователь: {user}", 
                            command.Value.Name, context.User);
                        break;
                    case CommandError.BadArgCount:
                        reason = $"{context.User.Mention}, неверное количество аргументов.\n" +
                            $"Напишите `help {command.Value.Name}`, чтобы посмотреть использование команды.";
                        _logger.Warning("Неверное количество аргументов в команде \"{command}\". Пользователь: {user}", 
                            command.Value.Name, context.User);
                        break;
                    case CommandError.ObjectNotFound:
                        reason = result.ErrorReason;
                        _logger.Warning(result.ErrorReason);
                        break;
                    case CommandError.MultipleMatches:
                        reason = result.ErrorReason;
                        _logger.Warning(result.ErrorReason);
                        break;
                    case CommandError.UnmetPrecondition:
                        reason = result.ErrorReason;
                        _logger.Warning(result.ErrorReason);
                        break;
                    case CommandError.Exception:
                        reason = $"При выполнении команды возникла ошибка.";
                        _logger.Error("При выполнении команды {command} возникла ошибка. Пользователь: {user}\n{error}",
                            command.Value.Name, context.User, result.ErrorReason);
                        break;
                    case CommandError.Unsuccessful:
                        reason = result.ErrorReason;
                        _logger.Warning(result.ErrorReason);
                        break;
                }

                var embed = _embedService.GetErrorEmbed("Ошибка!", reason);
                await context.Channel.SendMessageAsync(embed: embed as Embed);
            }

            if (result.Error.HasValue == false)
            {
                var commandName = command.IsSpecified ? command.Value.Name : "Неизвестная команда";
                var user = context.User;
                _logger.Information("{commandName} была вызвана пользователем {user}.", commandName, user);
            }
        }

        public async Task LogFromMessageAsync(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    _logger.Fatal(msg.Exception, "[{source}] {message}", msg.Source, msg.Message);
                    break;
                case LogSeverity.Error:
                    _logger.Error(msg.Exception, "[{source}] {message}", msg.Source, msg.Message);
                    break;
                case LogSeverity.Warning:
                    _logger.Warning(msg.Exception, "[{source}] {message}", msg.Source, msg.Message);
                    break;
                case LogSeverity.Info:
                    _logger.Information(msg.Exception, "[{source}] {message}", msg.Source, msg.Message);
                    break;
                case LogSeverity.Verbose:
                    _logger.Verbose(msg.Exception, "[{source}] {message}", msg.Source, msg.Message);
                    break;
                case LogSeverity.Debug:
                    _logger.Debug(msg.Exception, "[{source}] {message}", msg.Source, msg.Message);
                    break;
            }
        }
    }
}
