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
        private readonly LinkService _linkService;

        public CustomModule(DiscordSocketClient client, EmbedService embedService,
            TeaBagCommandService commandManager, ResponseParser responseParser,
            ResponseService responseService, GamesListService gameListService,
            LinkService linkService)
        {
            _client = client;
            _embedService = embedService;
            _commandManager = commandManager;
            _responseParser = responseParser;
            _responseService = responseService;
            _gameListService = gameListService;
            _linkService = linkService;
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

        [Command("youtube")]
        [Alias("yt", "ютуб")]
        public async Task YouTube()
        {
            Embed embed = null;
            var url = await _linkService.GetUrlAsync("youtube");

            if (url == null)
                embed = _embedService.GetErrorEmbed("Ошибка!", $"Ссылка не найдена.");
            else
                embed = _embedService.GetInfoEmbed("Канал на ютубе", url);

            await ReplyAsync(embed: embed);
        }

        [Command("twitch")]
        [Alias("твич")]
        public async Task Twitch()
        {
            Embed embed = null;
            var url = await _linkService.GetUrlAsync("twitch");

            if (url == null)
                embed = _embedService.GetErrorEmbed("Ошибка!", $"Ссылка не найдена.");
            else
                embed = _embedService.GetInfoEmbed("Канал на твиче", url);

            await ReplyAsync(embed: embed);
        }

        [Command("steam")]
        [Alias("стим")]
        public async Task Steam()
        {
            Embed embed = null;
            var url = await _linkService.GetUrlAsync("steam");

            if (url == null)
                embed = _embedService.GetErrorEmbed("Ошибка!", $"Ссылка не найдена.");
            else
                embed = _embedService.GetInfoEmbed("Страница в стиме", url);

            await ReplyAsync(embed: embed);
        }

        [Command("discord")]
        [Alias("дискорд", "инвайт", "invite")]
        public async Task Discord()
        {
            Embed embed = null;
            var url = await _linkService.GetUrlAsync("discord");

            if (url == null)
                embed = _embedService.GetErrorEmbed("Ошибка!", $"Ссылка не найдена.");
            else
                embed = _embedService.GetInfoEmbed("Ссылка на инвайт", url);

            await ReplyAsync(embed: embed);
        }

        [Command("donate")]
        [Alias("донат")]
        public async Task Donate()
        {
            Embed embed = null;
            var url = await _linkService.GetUrlAsync("donate");

            if (url == null)
                embed = _embedService.GetErrorEmbed("Ошибка!", $"Ссылка не найдена.");
            else
                embed = _embedService.GetInfoEmbed("Ссылка на донат", url);

            await ReplyAsync(embed: embed);
        }

        [Command("vk")]
        [Alias("вк")]
        public async Task VK()
        {
            Embed embed = null;
            var url = await _linkService.GetUrlAsync("vk");

            if (url == null)
                embed = _embedService.GetErrorEmbed("Ошибка!", $"Ссылка не найдена.");
            else
                embed = _embedService.GetInfoEmbed("Ссылка на группу ВК", url);

            await ReplyAsync(embed: embed);
        }
    }
}
