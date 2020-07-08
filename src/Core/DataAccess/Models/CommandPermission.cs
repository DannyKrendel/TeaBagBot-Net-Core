using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.Options;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace TeaBagBot.DataAccess.Models
{
    [BsonCollection("CommandPermissions")]
    public class CommandPermission
    {
        public string Command { get; set; }
        [ValueSerializer(typeof(int))]
        public GuildPermission Permissions { get; set; }
    }
}
