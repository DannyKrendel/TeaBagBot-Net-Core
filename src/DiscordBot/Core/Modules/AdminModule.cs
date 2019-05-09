using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core.Attributes;
using DiscordBot.Core.Entities;
using System.Threading.Tasks;

namespace DiscordBot.Core.Modules
{
    [RequirePermissions(PermissionGroup.Admin)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient client;
        private readonly EmbedService embedService;
        private readonly CommandManager commandManager;
        private readonly ConfigService configService;

        public AdminModule(DiscordSocketClient client, EmbedService embedService, CommandManager commandManager, ConfigService configService)
        {
            this.client = client;
            this.embedService = embedService;
            this.commandManager = commandManager;
            this.configService = configService;
        }

        [CustomCommand("prefix")]
        [CustomAlias("prefix")]
        public async Task Prefix(string newPrefix = null)
        {
            if (newPrefix == null)
            {
                await ReplyAsync($"Текущий префикс: `{configService.LoadConfig().Prefix}`");
            }
            else if (newPrefix.Length >= 2)
            {
                await ReplyAsync($"{Context.User.Mention}, длина префикса не должна превышать 2 символа.");
            }
            else
            {
                var config = configService.LoadConfig();
                config.Prefix = newPrefix;
                configService.SaveConfig(config);

                await ReplyAsync($"{Context.User.Mention}, префикс изменён на `{newPrefix}`");
            }
        }
    }
}
