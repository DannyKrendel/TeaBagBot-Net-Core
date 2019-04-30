using DiscordBot.Storage.Interfaces;

namespace DiscordBot
{
    public static class TokenManager
    {
        private static readonly string tokenPath = @"C:\Users\Danny\Source\Repos\DiscordBot\src\DiscordBot\Config\Token";

        public static string GetToken()
        {
            return Unity.Resolve<IDataStorage>().RestoreObject<string>(tokenPath);
        }
    }
}
