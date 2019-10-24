using TeaBagBot.ConsoleApp.Logging;
using TeaBagBot.Core.Logging;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class LoggerTests
    {
        [Theory]
        [InlineData("test")]
        [InlineData("")]
        [InlineData(null)]
        public void Log_ShouldLog_IfValidMessage(string msg)
        {
            var logger = new ConsoleLogger();

            var ex = Record.Exception(() => logger.Log(new BotLogMessage(BotLogSeverity.Info, null, msg)));

            Assert.Null(ex);
        }
    }
}
