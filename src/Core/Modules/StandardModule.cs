using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeaBagBot.Helpers;
using TeaBagBot.Messages;
using TeaBagBot.Attributes;
using TeaBagBot.Services;
using TeaBagBot.DataAccess.Models;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TeaBagBot.Modules
{
    public class StandardModule : ModuleBase
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly TeaBagCommandService _commandManager;
        private readonly ResponseParser _responseParser;
        private readonly ResponseService _responseService;
        private readonly GamesListService _gameListService;

        public StandardModule(DiscordSocketClient client, EmbedService embedService, 
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

        [TeaBagCommand, Aliases, Description, UserPermission]
        public async Task Help(string commandName = null)
        {
            string GetCommandInfo(TeaBagCommand command)
            {
                return $"{command.Name}\n";
            }

            string GetFormattedArray(string[] arr)
            {
                string result = "";
                for (int i = 0; i < arr.Length; i++)
                {
                    result += arr[i];
                    if (i != arr.Length - 1)
                        result += ", ";
                }

                return result;
            }

            Embed embed = null;

            if (commandName == null)
            {
                string commandInfo = $"**{_commandManager.GetCommand("help").Description}**\n";

                commandInfo += "Список стандартных команд:\n```\n";
                foreach (var command in _commandManager.GetCommandsInGroup(ModuleGroup.Standard).OrderBy(s => s.Name))
                {
                    commandInfo += GetCommandInfo(command);
                }
                commandInfo += "\n```\n";
                commandInfo += "Список админских команд:\n```\n";
                foreach (var command in _commandManager.GetCommandsInGroup(ModuleGroup.Admin).OrderBy(s => s.Name))
                {
                    commandInfo += GetCommandInfo(command);
                }
                commandInfo += "\n```\n";

                embed = _embedService.GetInfoEmbed("Помощь", commandInfo);
            }
            else
            {
                var command = _commandManager.GetCommand(commandName);

                if (command == null)
                {
                    embed = _embedService.GetErrorEmbed("Ошибка!", $"{Context.User.Mention}, команды \"{commandName}\" не существует.");
                }
                else
                {
                    string description = "Описание:" +
                        "\n```\n" +
                        command.Description +
                        "\n```\n" +
                        "Псевдонимы:" +
                        "\n```\n" +
                        GetFormattedArray(command.Aliases) +
                        "\n```\n";
                    embed = _embedService.GetInfoEmbed(command.Name, description);
                }
            }

            await ReplyAsync(embed: embed);
        }

        [TeaBagCommand, Aliases, Description, UserPermission]
        public async Task Say([Remainder] string message)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(message);
        }

        [TeaBagCommand, Aliases, Description, UserPermission]
        public async Task Ping()
        {
            await ReplyAsync($"{Context.User.Mention}, понг! ({_client.Latency}мс)");
        }

        [TeaBagCommand, Aliases, Description, UserPermission]
        public async Task Ball([Remainder] string question)
        {
            string rawResponse = await _responseService.GetRandomResponseByCommandNameAsync("8ball");
            Embed embed = null;

            if (rawResponse != null)
            {
                string response = _responseParser.Parse(rawResponse, new TeaBagMessageContext(Context.Message));
                embed = _embedService.GetInfoEmbed(":8ball: Предсказание :8ball:", response);
            }
            else
            {
                embed = _embedService.GetErrorEmbed("Ошибка!", "Не найдено ответов.");
            }

            await ReplyAsync(embed: embed);
        }

        [TeaBagCommand, Aliases, Description, UserPermission]
        public async Task Pick([Remainder] string message)
        {
            string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            Embed embed = null;

            if (options.Length == 0)
            {
                embed = _embedService.GetErrorEmbed("Ошибка!", "Не найдено вариантов.");
            }
            else
            {
                string selection = RandomUtils.GetRandomFrom(options);
                embed = _embedService.GetInfoEmbed(":eyes: Выбор :eyes:", $"Мой выбор пал на... `{selection}`");
            }

            await ReplyAsync(embed: embed);
        }

        [TeaBagCommand, Aliases, Description, UserPermission]
        public async Task Poll([Remainder] string message)
        {
            var emotes = new List<IEmote>();
            var args = Regex.Split(message, @"^\'|\'\s+\'|\'+$", RegexOptions.Multiline).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            var question = args[0];
            var options = args.Skip(1).ToArray();

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
            var newOptions = options.Where(str => str.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length == 1).ToArray();
            if (newOptions.Length == options.Length)
                msg = await ReplyAsync($"**{question}**");
            else
                msg = await ReplyAsync($"**{question}**\n{string.Concat(options.Select(o => o + "\n"))}");
            await msg.AddReactionsAsync(emotes.ToArray());
        }
    }
}
