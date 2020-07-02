using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.DataAccess.Models
{
    [BsonCollection("BotConfigs")]
    public class BotConfig : EntityBase
    {
        public ulong GuildId { get; set; }
        public string Prefix { get; set; }
        public ulong DefaultChannelId { get; set; }
    }
}
