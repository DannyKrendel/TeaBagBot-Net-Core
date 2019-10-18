using TeaBagBot.Core;
using Xunit;

namespace TeaBagBot.Tests.Unit
{
    public class UnityTests
    {
        [Fact]
        public void RegisterTypes_ShouldNotThrow()
        {
            var ex = Record.Exception(() => Unity.RegisterTypes());

            Assert.Null(ex);
        }
    }
}
