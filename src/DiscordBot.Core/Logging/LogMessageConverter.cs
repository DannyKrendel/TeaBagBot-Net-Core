using Discord;

namespace TeaBagBot.Core.Logging
{
    public static class LogMessageConverter
    {
        public static BotLogMessage ToBotLogMessage(LogMessage logMsg)
        {
            var botLogMsg = new BotLogMessage(default, logMsg.Source, logMsg.Message, logMsg.Exception);

            switch (logMsg.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    botLogMsg.Severity = BotLogSeverity.Error;
                    break;
                case LogSeverity.Warning:
                    botLogMsg.Severity = BotLogSeverity.Warning;
                    break;
                case LogSeverity.Info:
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    botLogMsg.Severity = BotLogSeverity.Info;
                    break;
            }

            return botLogMsg;
        }
    }
}
