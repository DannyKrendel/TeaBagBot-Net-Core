using DiscordBot.Storage.Interfaces;
using System;

namespace DiscordBot
{
    public static class TokenManager
    {
        private static readonly string tokenPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Config\Token";

        public static string GetToken()
        {
            try
            {
                return Unity.Resolve<IDataStorage>().RestoreObject<string>(tokenPath);
            }
            catch (Exception ex)
            {
                throw new TokenException($"Couldn't load token from '{tokenPath}'.", ex);
            }
        }
    }
}
