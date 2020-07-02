using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.DataAccess.Models
{
    [BsonCollection("ResponseInfos")]
    public class ResponseInfo : EntityBase
    {
        public string CommandName { get; set; }
        public string Pattern { get; set; }
        public string[] Responses { get; set; }
    }
}
