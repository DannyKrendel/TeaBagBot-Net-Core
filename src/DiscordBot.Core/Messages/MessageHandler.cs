using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeaBagBot.Core.Logging;

namespace TeaBagBot.Core.Messages
{
    public class MessageHandler : IMessageHandler<SocketMessage>
    {
        private readonly DiscordLogger _logger;
        private readonly DiscordSocketClient _client;
        private readonly ResponseParser _responseParser;
        private readonly ResponseProvider _responseProvider;

        public MessageHandler(DiscordLogger logger, DiscordSocketClient client,
            ResponseParser responseParser, ResponseProvider responseProvider)
        {
            _logger = logger;
            _client = client;
            _responseParser = responseParser;
            _responseProvider = responseProvider;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandleMessageAsync;
        }

        public async Task HandleMessageAsync(SocketMessage message)
        {
            var msg = message as SocketUserMessage;
            int argPos = 0;

            if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                string rawResponse = _responseProvider.GetRandomResponseByMessage(msg.Content.Substring(argPos));
                if (string.IsNullOrEmpty(rawResponse) == false)
                {
                    string response = _responseParser.Parse(rawResponse, new TeaBagMessageContext(msg));
                    await msg.Channel.SendMessageAsync(response);
                }
            }
        }
    }
}
