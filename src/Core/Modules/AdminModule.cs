using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using TeaBagBot.Attributes;
using TeaBagBot.Services;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;

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
    }
}
