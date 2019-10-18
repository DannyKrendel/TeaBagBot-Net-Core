using TeaBagBot.Core.Factories;
using Xunit;

namespace TeaBagBot.Tests.Unit
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
