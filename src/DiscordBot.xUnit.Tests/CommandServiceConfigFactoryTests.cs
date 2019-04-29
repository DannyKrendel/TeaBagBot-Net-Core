using DiscordBot.Core;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class CommandServiceConfigFactoryTests
    {
        [Fact]
        public void GetDefault_ShouldWork()
        {
            var config = CommandServiceConfigFactory.GetDefault();

            Assert.NotNull(config);
        }

        [Fact]
        public void GetNew_ShouldWork()
        {
            var config = CommandServiceConfigFactory.GetNew();

            Assert.NotNull(config);
        }
    }
}
