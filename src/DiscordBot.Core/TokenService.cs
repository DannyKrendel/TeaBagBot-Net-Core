using DiscordBot.Storage;
using System;

namespace DiscordBot
{
    public class TokenService
    {
        private readonly IDataStorage _storage;

        public TokenService(IDataStorage storage)
        {
            this._storage = storage;
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
