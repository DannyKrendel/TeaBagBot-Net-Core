using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaBagBot
{
    public class MessagePaginator
    {
        private readonly IUser _author;
        private readonly IMessageChannel _channel;
        private readonly IList<Embed> _items;
        private readonly Emote _backEmote;
        private readonly Emote _forwardEmote;

        private int _currentPage;
        private IUserMessage _initialMessage;

        public MessagePaginator(IUser author, IMessageChannel channel, IList<Embed> items)
        {
            _author = author;
            _channel = channel;
            _items = items;
        }

        public async Task ShowFirstPageAsync()
        {
            _currentPage = 0;
            _initialMessage = await _channel.SendMessageAsync(embed: _items[_currentPage]);

            await AddReactionsAsync();
        }

        public async Task ShowPreviousPageAsync()
        {
            if (_currentPage == 0 || _initialMessage == null)
                return;

            _currentPage--;

            await ModifyCurrentPageAsync();
        }

        public async Task ShowNextPageAsync()
        {
            if (_currentPage == _items.Count - 1 || _initialMessage == null)
                return;

            _currentPage++;

            await ModifyCurrentPageAsync();
        }

        public async Task OnReactionAddedAsync(SocketReaction reaction)
        {
            if (reaction.UserId == _author.Id)
            {
                if (reaction.Emote == _backEmote)
                    await ShowPreviousPageAsync();
                else if (reaction.Emote == _forwardEmote)
                    await ShowNextPageAsync();
            }
        }

        private async Task ModifyCurrentPageAsync()
        {
            await _initialMessage.ModifyAsync(x => x.Embed = _items[_currentPage]);
        }

        private async Task AddReactionsAsync()
        {
            if (_initialMessage == null)
                return;

            await _initialMessage.AddReactionsAsync(new Emote[] { _backEmote, _forwardEmote });
        }
    }
}
