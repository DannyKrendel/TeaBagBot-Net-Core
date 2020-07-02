using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TeaBagBot.Commands;
using TeaBagBot.Models;

namespace TeaBagBot.ConsoleApp
{
    public class Startup
    {
        private readonly IBot _bot;
        private readonly ICommandHandler<string> _commandHandler;

        public Startup(IBot bot, ICommandHandler<string> commandHandler)
        {
            _bot = bot;
            _commandHandler = commandHandler;
        }

        public async Task InitializeAsync()
        {
            await _bot.StartAsync();
            await _commandHandler.InitializeAsync();
        }
    }
}
