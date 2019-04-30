using DiscordBot.Core;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DiscordBot.xUnit.Tests
{
    public class CommandHandlerTests
    {
        [Fact]
        public async Task HandleMessage_ShouldThrowArgumentNullException_IfSocketMessageIsNull()
        {
            var comHandler = Unity.Resolve<CommandHandler>();

            var argException = await Record.ExceptionAsync(async () => await comHandler.HandleMessageAsync(null));

            Assert.IsType<ArgumentNullException>(argException);
        }
    }
}
