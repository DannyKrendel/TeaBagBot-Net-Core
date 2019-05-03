using Xunit;

namespace DiscordBot.xUnit.Tests
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

            var ex = Record.Exception(() => logger.Log(msg));

            Assert.Null(ex);
        }
    }
}
