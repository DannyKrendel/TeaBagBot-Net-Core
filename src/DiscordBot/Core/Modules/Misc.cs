using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot.Core.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            await Context.Channel.SendMessageAsync(message);
        }
    }
}
