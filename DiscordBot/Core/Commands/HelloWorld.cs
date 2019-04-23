using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;

namespace DiscordBot.Core.Commands
{
    public class HelloWorld : ModuleBase<SocketCommandContext>
    {
        [Command("hello"), Alias("привет"), Summary("Бесполезная команда")]
        public async Task Hello()
        {
            await Context.Channel.SendMessageAsync("Привет мир");
        }
    }
}
