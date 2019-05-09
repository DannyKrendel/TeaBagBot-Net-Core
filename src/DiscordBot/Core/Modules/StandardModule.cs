using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core.Attributes;
using DiscordBot.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Core.Modules
{
    [RequirePermissions(PermissionGroup.Standard)]
    public class StandardModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient client;
        private readonly EmbedService embedService;
        private readonly CommandManager commandManager;
        private readonly ConfigService configService;

        public StandardModule(DiscordSocketClient client, EmbedService embedService, CommandManager commandManager, ConfigService configService)
        {
            this.client = client;
            this.embedService = embedService;
            this.commandManager = commandManager;
            this.configService = configService;
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
                string commandInfo = $"**{commandManager.GetCommand("help").Description}**\n";

                commandInfo += "Список стандартных команд:\n```\n";
                foreach (var command in commandManager.GetCommands(PermissionGroup.Standard).OrderBy(s => s.Name))
                {
                    commandInfo += GetCommandInfo(command);
                }
                commandInfo += "\n```\n";
                commandInfo += "Список админских команд:\n```\n";
                foreach (var command in commandManager.GetCommands(PermissionGroup.Admin).OrderBy(s => s.Name))
                {
                    commandInfo += GetCommandInfo(command);
                }
                commandInfo += "\n```\n";

                embed = embedService.GetInfoEmbed("Помощь", commandInfo);
            }
            else
            {
                var command = commandManager.GetCommand(commandName);

                if (command == null)
                {
                    embed = embedService.GetErrorEmbed("Ошибка!", $"{Context.User.Mention}, такой команды не существует.");
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
                    embed = embedService.GetInfoEmbed(command.Name, description);
                }
            }

            await ReplyAsync(embed: embed);
        }

        [CustomCommand("echo")]
        [CustomAlias("echo")]
        public async Task Echo([Remainder]string message)
        {
            var embed = embedService.GetInfoEmbed(message, "");

            await ReplyAsync(message);
        }

        [CustomCommand("ping")]
        [CustomAlias("ping")]
        public async Task Ping()
        {
            await ReplyAsync($"{Context.User.Mention}, понг! ({client.Latency}мс)");
        }

        [CustomCommand("8ball")]
        [CustomAlias("8ball")]
        public async Task Ball([Remainder]string question)
        {
            var responses = commandManager.GetCommand("8ball").Responses;
            Embed embed = null;

            if (responses.Count() == 0)
            {
                embed = embedService.GetErrorEmbed("Ошибка!", "Ответов не нашлось.");
            }
            else
            {
                string response = responses[RandomGenerator.GetRandom(0, responses.Count())];
                embed = embedService.GetInfoEmbed(":8ball:", response);
            }

            await ReplyAsync(embed: embed);
        }
    }
}
