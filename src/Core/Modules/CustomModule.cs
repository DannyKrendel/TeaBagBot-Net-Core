using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeaBagBot.Attributes;
using TeaBagBot.Messages;
using TeaBagBot.Services;

namespace TeaBagBot.Modules
{
    [RequirePermissions(PermissionGroup.Standard)]
    public class CustomModule : ModuleBase
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly TeaBagCommandService _commandManager;
        private readonly ResponseParser _responseParser;
        private readonly ResponseService _responseService;
        private readonly GamesListService _gameListService;

        public CustomModule(DiscordSocketClient client, EmbedService embedService,
            TeaBagCommandService commandManager, ResponseParser responseParser,
            ResponseService responseService, GamesListService gameListService)
        {
            _client = client;
            _embedService = embedService;
            _commandManager = commandManager;
            _responseParser = responseParser;
            _responseService = responseService;
            _gameListService = gameListService;
        }

        [Command("game")]
        [Alias("игра")]
        public async Task Game([Remainder] string gameTitle)
        {
            Embed embed = null;
            var gameInfo = await _gameListService.FindGameAsync(gameTitle);

            if (gameInfo == null)
            {
                embed = _embedService.GetErrorEmbed("Ошибка!", $"Игра \"{gameTitle}\" не найдена.");
            }
            else
            {
                string content = $"Статус: {gameInfo.Status}\nОписание: {gameInfo.Description}";
                if (string.IsNullOrEmpty(gameInfo.Url) == false)
                    content += $"\nПлейлист: {gameInfo.Url}";
                embed = _embedService.GetInfoEmbed(gameInfo.Name, content, url: gameInfo.Url);
            }

            await ReplyAsync(embed: embed);
        }
    }
}
