using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using TeaBagBot.Attributes;
using TeaBagBot.Services;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;
using Discord;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;

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
        public async Task Poll([Remainder] string message)
        {
            var emotes = new List<IEmote>();
            var args = Regex.Split(message, @"^\'|(?<=\S)\s+\'|\'\s+\'|\'+$", RegexOptions.Multiline).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            string modifier = args[0];
            string question;
            string[] options;

            if (modifier == "-e")
            {
                question = "@everyone\n" + args[1];
                options = args.Skip(2).ToArray();
            }
            else if (modifier == "-h")
            {
                question = "@here\n" + args[1];
                options = args.Skip(2).ToArray();
            }
            else
            {
                question = args[0];
                options = args.Skip(1).ToArray();
            }

            if (args.Length <= 1)
            {
                var embed = _embedService.GetErrorEmbed("Ошибка!", $"Неверное использование команды. Напишите `help poll`");
                await ReplyAsync(embed: embed);
                return;
            }

            for (int i = 0; i < options.Length; i++)
            {
                var wordsArr = options[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string firstString = wordsArr[0];

                if (firstString.Length > 0 && int.TryParse(firstString.TakeWhile(c => char.IsDigit(c)).ToArray(), out int number) && number >= 0 && number <= 10)
                {
                    switch (number)
                    {
                        case 0:
                            emotes.Add(new Emoji("0\u20e3"));
                            break;
                        case 1:
                            emotes.Add(new Emoji("1\u20e3"));
                            break;
                        case 2:
                            emotes.Add(new Emoji("2\u20e3"));
                            break;
                        case 3:
                            emotes.Add(new Emoji("3\u20e3"));
                            break;
                        case 4:
                            emotes.Add(new Emoji("4\u20e3"));
                            break;
                        case 5:
                            emotes.Add(new Emoji("5\u20e3"));
                            break;
                        case 6:
                            emotes.Add(new Emoji("6\u20e3"));
                            break;
                        case 7:
                            emotes.Add(new Emoji("7\u20e3"));
                            break;
                        case 8:
                            emotes.Add(new Emoji("8\u20e3"));
                            break;
                        case 9:
                            emotes.Add(new Emoji("9\u20e3"));
                            break;
                        case 10:
                            emotes.Add(new Emoji("\uD83D\uDD1F"));
                            break;
                    }
                }
                else
                {
                    IEmote emote;
                    if (firstString.StartsWith('<') && firstString.EndsWith('>'))
                    {
                        Emote.TryParse(firstString, out Emote e);
                        emote = e;
                    }
                    else
                    {
                        emote = new Emoji(EmojiOne.EmojiOne.ShortnameToUnicode(firstString));
                    }

                    if (emote != null)
                        emotes.Add(emote);
                }
            }

            await Context.Message.DeleteAsync();
            IUserMessage msg;
            int sameCount = options.Count(str => str.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length == 1);
            if (options.Length == sameCount)
                msg = await ReplyAsync($"**{question}**");
            else
                msg = await ReplyAsync($"**{question}**\n{string.Concat(options.Select(o => o + "\n"))}");
            await msg.AddReactionsAsync(emotes.ToArray());
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
