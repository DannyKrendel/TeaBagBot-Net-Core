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
    }
}
