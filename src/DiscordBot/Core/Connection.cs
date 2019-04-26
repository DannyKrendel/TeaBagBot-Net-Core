﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Core.Entities;

namespace DiscordBot.Core
{
    public class Connection
    {
        private readonly DiscordSocketClient client;
        private readonly DiscordLogger logger;

        public Connection(DiscordLogger logger, DiscordSocketClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        internal async Task ConnectAsync(BotConfig config)
        {
            client.Log += logger.Log;

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
    }
}