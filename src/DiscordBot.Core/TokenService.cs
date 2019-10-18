using TeaBagBot.Core.Storage;
using System;

namespace TeaBagBot.Core
{
    public class TokenService
    {
        private readonly IDataStorage _storage;

        public TokenService(IDataStorage storage)
        {
            _storage = storage;
        }

        public string GetToken()
        {
            try
            {
                return _storage.RestoreObject<string>("Token");
            }
            catch (Exception ex)
            {
                throw new TokenException($"Couldn't load token.", ex);
            }
        }
    }
}
