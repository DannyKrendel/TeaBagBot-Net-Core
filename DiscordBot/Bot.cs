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
                CaseSensitiveCommands = false,
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
            Console.WriteLine($"[{DateTime.Now} at {msg.Source}] {msg.Message}");
        }

        private async Task Client_Ready()
        {
            await client.SetGameAsync("игру");
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            
        }
    }
}
