using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TeaBagBot.Core.Commands;
using TeaBagBot.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeaBagBot.Core.Helpers;
using TeaBagBot.Core.Messages;
using TeaBagBot.Core;
using TeaBagBot.Discord.Messages;
using TeaBagBot.Discord.Attributes;

namespace TeaBagBot.Discord.CommandModules
{
    [RequirePermissions(PermissionGroup.Standard)]
    public class StandardModule : ModuleBase
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;
        private readonly TeaBagCommandProvider _commandManager;
        private readonly ConfigService _configService;
        private readonly ResponseParser _responseParser;
        private readonly ResponseProvider _responseProvider;

        public StandardModule(DiscordSocketClient client, EmbedService embedService, TeaBagCommandProvider commandManager,
            ConfigService configService, ResponseParser responseParser, ResponseProvider responseProvider)
        {
            _client = client;
            _embedService = embedService;
            _commandManager = commandManager;
            _configService = configService;
            _responseParser = responseParser;
            _responseProvider = responseProvider;
        }

        [Command("help")]
        [Alias("помощь", "помоги", "хелп")]
        public async Task Help([Remainder]string commandName = null)
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

        [Command("say")]
        [Alias("echo", "скажи", "повтори")]
        public async Task Say([Remainder]string message)
        {
            var embed = _embedService.GetInfoEmbed(message, "");

            await Context.Message.DeleteAsync();
            await ReplyAsync(message);
        }

        [Command("ping")]
        [Alias("пинг")]
        public async Task Ping()
        {
            await ReplyAsync($"{Context.User.Mention}, понг! ({_client.Latency}мс)");
        }

        [Command("8ball")]
        [Alias("шар", "предсказание")]
        public async Task Ball([Remainder]string question)
        {
            string rawResponse = _responseProvider.GetRandomResponseByName("8ball");
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

        [Command("pick")]
        [Alias("выбор", "выбери")]
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
