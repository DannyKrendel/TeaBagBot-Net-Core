using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DiscordBot
{
    class Bot
    {
        private DiscordSocketClient client;
        private CommandService commands;

        public async Task MainAsync()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            client.MessageReceived += Client_MessageReceived;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            client.Ready += Client_Ready;
            client.Log += Client_Log;

            string token = "";
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.1", @"Data\Token.txt");
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var tokenReader = new StreamReader(stream))
                {
                    token = tokenReader.ReadToEnd();
                }
            }

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task Client_Log(LogMessage msg)
        {
            Log(msg.Source, msg.Message);
        }

        private async Task Client_Ready()
        {
            await client.SetGameAsync("игру");
        }

        private async Task Client_MessageReceived(SocketMessage socketMsg)
        {
            var userMsg = socketMsg as SocketUserMessage;
            var context = new SocketCommandContext(client, userMsg);

            if (context.Message == null || context.Message.Content == "")
                return;
            if (context.User.IsBot)
                return;

            int prefixPos = 0;

            if (userMsg.HasStringPrefix("x!", ref prefixPos, StringComparison.CurrentCultureIgnoreCase) == false && 
                userMsg.HasMentionPrefix(client.CurrentUser, ref prefixPos) == false)
            {
                return;
            }

            var result = await commands.ExecuteAsync(context, prefixPos, null);

            if (result.IsSuccess == false)
            {
                Log("Commands", $"Что-то пошло не так с командой. Текст: {context.Message.Content} | Причина ошибки: {result.ErrorReason}");
            }
        }

        private void Log(string source, string message)
        {
            Console.WriteLine($"[{DateTime.Now} at {source}] {message}");
        }
    }
}
