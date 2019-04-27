using System;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class LoggerTests
    {
        [Fact]
        public void BasicLogger_ThrowsException()
        {
            var logger = Unity.Resolve<ILogger>();

            Assert.Throws<ArgumentException>(() => logger.Log(null));

            var ex = Record.Exception(() => logger.Log("test"));

            Assert.Null(ex);
        }
    }
}
