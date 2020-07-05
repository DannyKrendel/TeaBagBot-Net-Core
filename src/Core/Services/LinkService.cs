using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;

namespace TeaBagBot.Services
{
    public class LinkService
    {
        private readonly IRepository<LinkInfo> _linkRepository;

        public LinkService(IRepository<LinkInfo> linkRepository)
        {
            _linkRepository = linkRepository;
        }

        public async Task<string> GetUrlAsync(string name)
        {
            var linkInfo = await _linkRepository.FindOneAsync(l => l.Name == name);

            if (linkInfo != null)
                return linkInfo.Url;
            return null;
        }
    }
}
