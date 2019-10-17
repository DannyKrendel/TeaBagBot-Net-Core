using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Core.Attributes;
using DiscordBot.Core.Entities;
using DiscordBot.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Core.Modules
{
    [RequirePermissions(PermissionGroup.Standard)]
    public class StandardModule : ModuleBase
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly CommandManager _commandManager;
        private readonly ConfigService _configService;
        private readonly CommandParser _commandParser;

        public StandardModule(DiscordSocketClient client, EmbedService embedService, CommandManager commandManager, ConfigService configService, CommandParser commandParser)
        {
            _client = client;
            _embedService = embedService;
            _commandManager = commandManager;
            _configService = configService;
            _commandParser = commandParser;
        }

        [CustomCommand("help")]
        [CustomAlias("help")]
        public async Task Help(string commandName = null)
        {
            string GetCommandInfo(CommandData command)
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
                foreach (var command in _commandManager.GetCommands(PermissionGroup.Standard).OrderBy(s => s.Name))
                {
                    commandInfo += GetCommandInfo(command);
                }
                commandInfo += "\n```\n";
                commandInfo += "Список админских команд:\n```\n";
                foreach (var command in _commandManager.GetCommands(PermissionGroup.Admin).OrderBy(s => s.Name))
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
                    embed = _embedService.GetErrorEmbed("Ошибка!", $"{Context.User.Mention}, такой команды не существует.");
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

        [CustomCommand("say")]
        [CustomAlias("say")]
        public async Task Say([Remainder]string message)
        {
            var embed = _embedService.GetInfoEmbed(message, "");

            await Context.Message.DeleteAsync();
            await ReplyAsync(message);
        }

        [CustomCommand("ping")]
        [CustomAlias("ping")]
        public async Task Ping()
        {
            await ReplyAsync($"{Context.User.Mention}, понг! ({_client.Latency}мс)");
        }

        [CustomCommand("8ball")]
        [CustomAlias("8ball")]
        public async Task Ball([Remainder]string question)
        {
            var responses = _commandManager.GetCommand("8ball").Responses;
            Embed embed = null;

            if (responses.Count() == 0)
            {
                embed = _embedService.GetErrorEmbed("Ошибка!", "Не найдено ответов.");
            }
            else
            {
                string response = _commandParser.Parse(RandomUtils.GetRandomFrom(responses), Context);
                embed = _embedService.GetInfoEmbed(":8ball: Предсказание :8ball:", response);
            }

            await ReplyAsync(embed: embed);
        }

        [CustomCommand("pick")]
        [CustomAlias("pick")]
        public async Task Pick([Remainder]string message)
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
