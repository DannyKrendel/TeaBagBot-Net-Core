using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class TokenManagerTests
    {
        [Fact]
        public void GetToken_ShouldWork()
        {
            string token = TokenManager.GetToken();

            Assert.NotNull(token);
        }
    }
}
