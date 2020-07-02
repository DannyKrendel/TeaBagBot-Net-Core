using TeaBagBot.DI;
using Unity;
using Xunit;

namespace TeaBagBot.UnitTests
{
    public class DITests
    {
        [Fact]
        public void RegisterCoreTypes_ShouldNotThrow()
        {
            var container = new UnityContainer();

            var ex = Record.Exception(() => container.RegisterCoreTypes());

            Assert.Null(ex);
        }
    }
}
