using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using System.Linq;
using System.Threading.Tasks;
using TeaBagBot.Services;

namespace TeaBagBot.Messages
{
    public class MessageHandler : IMessageHandler<SocketMessage>
    {
        private readonly ILogger _logger;
        private readonly DiscordSocketClient _client;
        private readonly ResponseParser _responseParser;
        private readonly ResponseService _responseService;

        public MessageHandler(ILogger logger, DiscordSocketClient client,
            ResponseParser responseParser, ResponseService responseService)
        {
            _logger = logger;
            _client = client;
            _responseParser = responseParser;
            _responseService = responseService;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandleMessageAsync;
        }

        public async Task HandleMessageAsync(SocketMessage message)
        {
            var msg = message as SocketUserMessage;
            int argPos = 0;

            if (msg.MentionedUsers.Contains(_client.CurrentUser))
            {
                string rawResponse = await _responseService.GetRandomResponseByMessageAsync(msg.Content.Substring(argPos));
                if (string.IsNullOrEmpty(rawResponse) == false)
                {
                    string response = _responseParser.Parse(rawResponse, new TeaBagMessageContext(msg));
                    await msg.Channel.SendMessageAsync(response);
                }
            }
        }
    }
}
