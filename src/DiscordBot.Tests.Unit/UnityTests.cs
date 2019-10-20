using TeaBagBot.ConsoleApp.DI;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class UnityTests
    {
        [Fact]
        public void RegisterTypes_ShouldNotThrow()
        {
            var ex = Record.Exception(() => UnityDI.RegisterTypes());

            Assert.Null(ex);
        }
    }
}
