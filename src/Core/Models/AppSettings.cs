using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using TeaBagBot.DataAccess;

namespace TeaBagBot.Models
{
    public class AppSettings
    {
        public string DiscordToken { get; set; }
        public IMongoDbSettings MongoDbSettings { get; set; }
    }
}
