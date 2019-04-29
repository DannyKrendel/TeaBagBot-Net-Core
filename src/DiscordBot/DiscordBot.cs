using DiscordBot.Core;
using DiscordBot.Core.Entities;
using DiscordBot.Storage.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class DiscordBot
    {
        private readonly IDataStorage storage;
        private readonly Connection connection;

        public DiscordBot(IDataStorage storage, Connection connection)
        {
            this.storage = storage;
            this.connection = connection;
        }

        public async Task Start()
        {
            await connection.ConnectAsync(TokenManager.GetToken());
        }
    }
}
