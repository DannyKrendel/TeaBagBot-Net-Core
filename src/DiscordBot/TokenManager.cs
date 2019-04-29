using DiscordBot.Storage.Interfaces;
using System;
using System.IO;


namespace DiscordBot
{
    public static class TokenManager
    {
        private static readonly string tokenPath = 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Config\Token");

        public static string GetToken()
        {
            return Unity.Resolve<IDataStorage>().RestoreObject<string>(tokenPath);
        }
    }
}
