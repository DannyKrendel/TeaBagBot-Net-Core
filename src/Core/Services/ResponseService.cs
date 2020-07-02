using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TeaBagBot.Helpers;
using TeaBagBot.DataAccess.Models;
using TeaBagBot.DataAccess;
using System.Threading.Tasks;

namespace TeaBagBot.Services
{
    public class ResponseService
    {
        private readonly IRepository<ResponseInfo> _responseRepository;

        public ResponseService(IRepository<ResponseInfo> responseRepository)
        {
            _responseRepository = responseRepository;
        }

        private async Task<string[]> GetByCommandNameAsync(string name)
        {
            return (await _responseRepository.FindOneAsync(r => r.CommandName == name))?.Responses;
        }

        private async Task<string[]> GetByPatternAsync(string pattern)
        {
            return (await _responseRepository.FindOneAsync(r => r.Pattern == pattern)).Responses;
        }

        public async Task<string> GetRandomResponseByCommandNameAsync(string name)
        {
            return RandomUtils.GetRandomFrom(await GetByCommandNameAsync(name));
        }

        public async Task<string> GetRandomResponseByMessageAsync(string message)
        {
            var foundResponses = new List<ResponseInfo>();
            Regex regex;

            foreach (var response in _responseRepository.AsQueryable())
            {
                if (string.IsNullOrEmpty(response.Pattern))
                    continue;
                regex = new Regex(response.Pattern);
                var match = regex.Match(message);
                if (match.Success)
                {
                    foundResponses.Add(response);
                }
            }

            if (foundResponses.Count == 0)
                return null;

            var defaultResponse = foundResponses.FirstOrDefault(r => r.Pattern == ".*");

            if (defaultResponse != null && foundResponses.Count > 1)
                foundResponses.Remove(defaultResponse);

            return RandomUtils.GetRandomFrom(RandomUtils.GetRandomFrom(foundResponses).Responses);
        }
    }
}
