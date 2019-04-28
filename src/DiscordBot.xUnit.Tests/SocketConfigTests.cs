using DiscordBot.Core;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class SocketConfigTests
    {
        [Fact]
        public void GetDefault_ShouldWork()
        {
            var config = SocketConfig.GetDefault();

            Assert.NotNull(config);
        }

        [Fact]
        public void GetNew_ShouldWork()
        {
            var config = SocketConfig.GetNew();

            Assert.NotNull(config);
        }
    }
}
