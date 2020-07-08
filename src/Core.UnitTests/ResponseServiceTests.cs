using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using TeaBagBot.Services;
using Xunit;
using TeaBagBot.DataAccess.Models;
using TeaBagBot.DataAccess;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System.Linq.Expressions;

namespace TeaBagBot.UnitTests
{
    public class ResponseServiceTests
    {
        private readonly ResponseService _responseService;

        public ResponseServiceTests()
        {
            var responses = new List<ResponseInfo>();
            responses.Add(new ResponseInfo
            {
                Pattern = "hello",
                Responses = new[] { "hello text" }
            });
            responses.Add(new ResponseInfo
            {
                Pattern = ".*",
                Responses = new[] { "all text" }
            });
            responses.Add(new ResponseInfo
            {
                CommandName = "Test",
                Pattern = "SomePattern",
                Responses = new[] { "1", "2", "3" }
            });

            var repoMock = new Mock<IRepository<ResponseInfo>>();
            repoMock.Setup(r => r.AsQueryable())
                .Returns(responses.AsQueryable());
            repoMock.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<ResponseInfo, bool>>>()))
                .Returns((Expression<Func<ResponseInfo, bool>> x) => Task.FromResult(responses.FirstOrDefault(x.Compile())));

            _responseService = new ResponseService(repoMock.Object);
        }

        [Theory]
        [InlineData("hello", "hello text")]
        [InlineData("random message", "all text")]
        public async Task GetRandomResponseByMessage_ReturnsExpected(string message, string expectedResponse)
        {
            string actualResponse = await _responseService.GetRandomResponseByMessageAsync(message);

            Assert.NotNull(actualResponse);
            Assert.Equal(expectedResponse, actualResponse);
        }

        [Theory]
        [InlineData("Test", "1", "2", "3")]
        public async Task GetRandomResponseByName_ReturnsExpected(string name, params string[] expectedResponses)
        {
            string actualResponse = await _responseService.GetRandomResponseByCommandNameAsync(name);

            Assert.NotNull(actualResponse);
            Assert.Contains(actualResponse, expectedResponses);
        }
    }
}
