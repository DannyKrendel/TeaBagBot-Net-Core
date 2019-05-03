using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot.Core.Modules
{
    public class MainModule : ModuleBase<SocketCommandContext>
    {
        private readonly EmbedService embedService;
        private readonly CommandManager commandManager;

        public MainModule(EmbedService embedService, CommandManager commandManager)
        {
            this.embedService = embedService;
            this.commandManager = commandManager;
        }

        [Command("help")]
        public async Task Help()
        {
            await ReplyAsync("Помощь уже в пути.");
        }

        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            var embed = embedService.GetInfoEmbed("Echoed Message", message);

            await ReplyAsync(embed: embed);
        }
    }
}
