using Discord;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TeaBagBot.DataAccess.Models
{
    [BsonCollection("Commands")]
    public class TeaBagCommand : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public ModuleGroup Group { get; set; }
    }
}
