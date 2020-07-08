using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using TeaBagBot.Attributes;
using TeaBagBot.Services;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;
using Discord;

namespace TeaBagBot.Modules
{
    public class AdminModule : ModuleBase
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly TeaBagCommandService _commandService;
        private readonly IRepository<BotConfig> _configRepository;

        public AdminModule(DiscordSocketClient client, EmbedService embedService, TeaBagCommandService commandService,
            IRepository<BotConfig> configRepository)
        {
            _client = client;
            _embedService = embedService;
            _commandService = commandService;
            _configRepository = configRepository;
        }

        [TeaBagCommand, Aliases, Description, UserPermission]
        public async Task Prefix(string newPrefix = null)
        {
            var config = await _configRepository.FindOneAsync(c => c.GuildId == Context.Guild.Id);
            if (newPrefix == null)
            {
                await ReplyAsync($"Текущий префикс: `{config.Prefix}`");
            }
            else if (newPrefix.Length > 3 || newPrefix.Length == 0)
            {
                await ReplyAsync($"{Context.User.Mention}, длина префикса должна быть от 1 до 3 символов.");
            }
            else
            {
                config.Prefix = newPrefix;
                await _configRepository.ReplaceOneAsync(config);

                await ReplyAsync($"{Context.User.Mention}, префикс изменён на `{newPrefix}`");
            }
        }

        [TeaBagCommand, Aliases, Description, UserPermission]
        public async Task Perm(string commandName, int? permissions = null)
        {
            Embed embed = null;

            if (string.IsNullOrEmpty(commandName))
            {
                embed = _embedService.GetErrorEmbed("Ошибка!", $"Неверное использование команды. Напишите `help perm`");
            }
            else
            {
                var command = _commandService.GetCommand(commandName);
                if (command == null)
                {
                    embed = _embedService.GetErrorEmbed("Ошибка!", $"Команды `{commandName}` не найдено.");
                }
                else
                {
                    if (permissions == null)
                    {
                        embed = _embedService.GetInfoEmbed($"Права команды {commandName}: ", _commandService.GetPermissions(command.Name).ToString());
                    }
                    else
                    {
                        await _commandService.ChangeCommandPermissions(commandName, permissions.Value);
                        embed = _embedService.GetInfoEmbed($"Права команды {commandName} успешно изменены.");
                    }
                }
            }

            await ReplyAsync(embed: embed);
        }
    }
}
