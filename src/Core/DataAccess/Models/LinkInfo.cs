using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.DataAccess.Models
{
    [BsonCollection("Links")]
    public class LinkInfo : EntityBase
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
