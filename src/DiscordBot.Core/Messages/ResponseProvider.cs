using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TeaBagBot.Core.Entities;
using TeaBagBot.Core.Helpers;

namespace TeaBagBot.Core.Messages
{
    public class ResponseProvider
    {
        public IReadOnlyCollection<TeaBagResponse> Responses { get; }

        public ResponseProvider(ResponseService responseService)
        {
            Responses = responseService.Load();
        }

        public string[] GetByName(string name)
        {
            return Responses.First(r => r.Name == name).Responses;
        }

        public string[] GetByPattern(string pattern)
        {
            return Responses.First(r => r.Pattern == pattern).Responses;
        }

        public string GetRandomResponseByName(string name)
        {
            return RandomUtils.GetRandomFrom(GetByName(name));
        }

        public string GetRandomResponseByMessage(string message)
        {
            Regex regex;

            foreach (var response in Responses)
            {
                if (string.IsNullOrEmpty(response.Pattern))
                    continue;
                regex = new Regex(response.Pattern);
                if (regex.IsMatch(message))
                {
                    return RandomUtils.GetRandomFrom(response.Responses);
                }
            }
            return null;
        }
    }
}
