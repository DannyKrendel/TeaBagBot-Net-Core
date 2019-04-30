using System;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class LoggerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Log_ShouldThrowArgumentException_IfInvalidMessage(string msg)
        {
            var logger = new Logger();

            var ex = Record.Exception(() => logger.Log(msg));

            Assert.IsAssignableFrom<ArgumentException>(ex);
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
