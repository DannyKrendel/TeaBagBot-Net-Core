using Moq;
using System.Threading.Tasks;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class DiscordBotTests
    {
        [Fact]
        public void Instantiation_ShouldWork()
        {
            var bot = Unity.Resolve<DiscordBot>();

            Assert.NotNull(bot);
        }
    }
}
