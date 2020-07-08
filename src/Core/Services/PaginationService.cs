using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaBagBot.Services
{
    public class PaginationService
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedService _embedService;

        public PaginationService(DiscordSocketClient client, EmbedService embedService)
        {
            _client = client;
            _embedService = embedService;
        }

        public MessagePaginator CreatePaginator(IMessage message, IList<string> strings, int maxItemsOnPage)
        {
            var embeds = new List<Embed>();

            for (int i = 0; i < maxItemsOnPage; i++)
            {
                if (i == strings.Count)
                    break;

                string pageTitle = $"Страница {i + 1}";
                string pageContent = strings.Select((x, j) => $"{j + 1}. {x}").Aggregate((x, y) => x + "\n" + y);
                embeds.Add(_embedService.GetInfoEmbed(pageTitle, pageContent));
            }

            var paginator = new MessagePaginator(message.Author, message.Channel, embeds);
            _client.ReactionAdded += async (msg, chnl, reaction) => { if (msg.Id == message.Id) await paginator.OnReactionAddedAsync(reaction); };

            return paginator;
        }
    }
}
