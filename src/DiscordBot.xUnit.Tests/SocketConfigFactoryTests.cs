using DiscordBot.Core.Factories;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class SocketConfigTests
    {
        [Fact]
        public void GetDefault_ShouldNotReturnNull()
        {
            var config = SocketConfigFactory.GetDefault();

            Assert.NotNull(config);
        }

        [Fact]
        public void GetNew_ShouldNotReturnNull()
        {
            var config = SocketConfigFactory.GetNew();

            Assert.NotNull(config);
        }
    }
}
