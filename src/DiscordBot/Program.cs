using DiscordBot.Core;
using DiscordBot.Core.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        private static async Task Main()
        {
            Unity.RegisterTypes();

            var storage = Unity.Resolve<Storage.Interfaces.IDataStorage>();

            var tokenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Config\Token");

            var connection = Unity.Resolve<Connection>();
            await connection.ConnectAsync(new BotConfig()
            {
                Token = storage.RestoreObject<string>(tokenPath)
            });

            Console.ReadKey();
        }
    }
}
