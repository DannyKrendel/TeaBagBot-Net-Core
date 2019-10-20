using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using TeaBagBot.Core.Entities;
using TeaBagBot.Core.Messages;
using TeaBagBot.Core.Storage;
using TeaBagBot.Core.Storage.Memory;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class ResponseProviderTests
    {
        private readonly IDataStorage _storage;
        private readonly string _path;
        private readonly List<TeaBagResponse> _responses;
        private readonly ResponseService _responseService;
        private readonly ResponseProvider _responseProvider;

        public ResponseProviderTests()
        {
            _storage = new MemoryStorage();

            _responses = new List<TeaBagResponse>();
            _responses.Add(new TeaBagResponse
            {
                Pattern = "hello",
                Responses = new[] { "hello text" }
            });
            _responses.Add(new TeaBagResponse
            {
                Pattern = ".*",
                Responses = new[] { "all text" }
            });
            _responses.Add(new TeaBagResponse
            {
                Name = "Test",
                Pattern = "SomePattern",
                Responses = new[] { "1", "2", "3" }
            });
            _storage.StoreObject(_responses, "Responses");

            _responseService = new ResponseService(_storage);
            _responseProvider = new ResponseProvider(_responseService);
        }

        [Theory]
        [InlineData("hello", "hello text")]
        [InlineData("random message", "all text")]
        public void ShouldGetRandomResponseByMessage(string message, string expectedResponse)
        {
            string actualResponse = _responseProvider.GetRandomResponseByMessage(message);

            Assert.NotNull(actualResponse);
            Assert.Equal(expectedResponse, actualResponse);
        }

        [Theory]
        [InlineData("Test", "1", "2", "3")]
        public void ShouldGetRandomResponseByName(string name, params string[] expectedResponses)
        {
            string actualResponse = _responseProvider.GetRandomResponseByName(name);

            Assert.NotNull(actualResponse);
            Assert.Contains(actualResponse, expectedResponses);
        }
    }
}
