using TeaBagBot.ConsoleApp;
using TeaBagBot.Messages;
using Moq;
using Xunit;

namespace TeaBagBot.UnitTests
{
    public class ResponseParserTests
    {
        [Theory]
        [InlineData("$AuthorUsername$, $AuthorMention$, $GuildName$, $ChannelName$", "a, b, c, d", "a", "b", "c", "d")]
        public void Parse_ShouldParse(string message, string expected, string username, string mention, string guildName, string channelName)
        {
            var responseParser = new ResponseParser();
            var contextStub = new Mock<ITeaBagMessageContext>();
            contextStub.Setup(m => m.AuthorUsername).Returns(username);
            contextStub.Setup(m => m.AuthorMention).Returns(mention);
            contextStub.Setup(m => m.GuildName).Returns(guildName);
            contextStub.Setup(m => m.ChannelName).Returns(channelName);

            string actual = responseParser.Parse(message, contextStub.Object);

            Assert.Equal(expected, actual);
        }
    }
}
