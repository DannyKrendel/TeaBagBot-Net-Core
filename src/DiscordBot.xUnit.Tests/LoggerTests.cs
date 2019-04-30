using System;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class LoggerTests
    {
        [Theory]
        [InlineData(null)]
        public void Log_ShouldThrow_IfMessageIsNull(string msg)
        {
            var logger = new Logger();

            var ex = Record.Exception(() => logger.Log(msg));

            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [InlineData("test")]
        public void Log_ShouldLog_IfValidMessage(string msg)
        {
            var logger = new Logger();

            var ex = Record.Exception(() => logger.Log(msg));

            Assert.Null(ex);
        }
    }
}
