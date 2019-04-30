using DiscordBot.Core;
using DiscordBot.Storage.Interfaces;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class DiscordBot
    {
        private readonly IDataStorage storage;
        private readonly Connection connection;
        private readonly CommandHandler commandHandler;

        public DiscordBot(IDataStorage storage, Connection connection, CommandHandler commandHandler)
        {
            this.storage = storage;
            this.connection = connection;
            this.commandHandler = commandHandler;
        }

        public async Task StartAsync()
        {
            await connection.ConnectAsync(TokenManager.GetToken());
            await commandHandler.InitializeAsync();
        }
    }
}
