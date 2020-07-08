using Discord;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
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
        public IList<CommandPermission> CommandPermissions { get; set; }
        public IList<ulong> EnabledChannels { get; set; }
    }
}
