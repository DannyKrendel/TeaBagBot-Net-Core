using DiscordBot.Storage.Implementations;
using DiscordBot.Storage.Interfaces;
using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class DataStorageTests
    {
        [Fact]
        public void DefaultStorageIsJsonStorage_ShouldWork()
        {
            var storage = Unity.Resolve<IDataStorage>();

            Assert.True(storage is JsonStorage);
        }
    }
}
