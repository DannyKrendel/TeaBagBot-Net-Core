using System;
using System.Threading.Tasks;
using DiscordBot.Core;
using DiscordBot.Core.Entities;
using DiscordBot.Storage;

namespace DiscordBot
{
    class Program
    {
        private static async Task Main()
        {
            Unity.RegisterTypes();

            var storage = Unity.Resolve<Storage.Interfaces.IDataStorage>();

            var tokenPath = @"C:\Users\Danny\Source\Repos\DiscordBot\DiscordBot\Config\Token";

            var connection = Unity.Resolve<Connection>();
            await connection.ConnectAsync(new BotConfig()
            {
                Token = storage.RestoreObject<string>(tokenPath)
            });

            Console.ReadKey();
        }
    }
}
