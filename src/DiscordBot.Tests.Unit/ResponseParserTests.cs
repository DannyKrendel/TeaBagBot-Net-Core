using TeaBagBot.ConsoleApp;
using TeaBagBot.Core.Messages;
using Moq;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class ResponseParserTests
    {
        private readonly Mock<ITeaBagMessageContext> _contextMock;
        private readonly ResponseParser _responseParser;

        public ResponseParserTests()
        {
            _contextMock = new Mock<ITeaBagMessageContext>();
            _responseParser = new ResponseParser();
        }

        [Theory]
        [InlineData("hello, $AuthorUsername$", "Anonymous", "hello, Anonymous")]
        [InlineData("$AuthorUsername$, hi", "абгдABCD1234", "абгдABCD1234, hi")]
        public void Username_ShouldParse(string message, string username, string expected)
        {
            _contextMock.Setup(m => m.AuthorUsername).Returns(username);

            string actual = _responseParser.Parse(message, _contextMock.Object);

            Assert.Equal(expected, actual);
        }
    }
}
