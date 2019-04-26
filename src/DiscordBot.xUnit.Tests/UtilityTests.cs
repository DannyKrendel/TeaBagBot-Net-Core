using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class UtilityTests
    {
        [Fact]
        public void MyFirstTest()
        {
            const int expected = 5;

            var actual = Utilities.MyUtility(5);

            Assert.Equal(expected, actual);
        }
    }
}
