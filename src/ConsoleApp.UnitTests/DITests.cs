using System;
using TeaBagBot.ConsoleApp.DI;
using Xunit;

namespace TeaBagBot.ConsoleApp.UnitTests
{
    public class DITests
    {
        [Fact]
        public void RegisterTypes_ShouldNotThrow()
        {
            var ex = Record.Exception(() => UnityDI.RegisterTypes());

            Assert.Null(ex);
        }
    }
}
