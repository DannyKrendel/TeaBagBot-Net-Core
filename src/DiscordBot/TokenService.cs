using DiscordBot.Storage.Interfaces;
using System;

namespace DiscordBot
{
    public class TokenService
    {
        private readonly IDataStorage storage;

        public TokenService(IDataStorage storage)
        {
            this.storage = storage;
        }

        public string GetToken()
        {
            try
            {
                return storage.RestoreObject<string>("Token");
            }
            catch (Exception ex)
            {
                throw new TokenException($"Couldn't load token.", ex);
            }
        }
    }
}
