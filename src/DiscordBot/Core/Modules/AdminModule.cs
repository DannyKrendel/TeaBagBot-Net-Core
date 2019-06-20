﻿using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core.Attributes;
using System.Threading.Tasks;

namespace DiscordBot.Core.Modules
{
    [RequirePermissions(PermissionGroup.Admin)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly CommandManager _commandManager;
        private readonly ConfigService _configService;

        public AdminModule(DiscordSocketClient client, EmbedService embedService, CommandManager commandManager, ConfigService configService)
        {
            _client = client;
            _embedService = embedService;
            _commandManager = commandManager;
            _configService = configService;
        }

        [CustomCommand("prefix")]
        [CustomAlias("prefix")]
        public async Task Prefix(string newPrefix = null)
        {
            if (newPrefix == null)
            {
                await ReplyAsync($"Текущий префикс: `{_configService.LoadConfig().Prefix}`");
            }
            else if (newPrefix.Length >= 2)
            {
                await ReplyAsync($"{Context.User.Mention}, длина префикса не должна превышать 2 символа.");
            }
            else
            {
                var config = _configService.LoadConfig();
                config.Prefix = newPrefix;
                _configService.SaveConfig(config);

                await ReplyAsync($"{Context.User.Mention}, префикс изменён на `{newPrefix}`");
            }
        }
    }
}
