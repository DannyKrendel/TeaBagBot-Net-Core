using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.Core.Entities
{
    public class TeaBagResponse
    {
        public string Name { get; set; }
        public string Pattern { get; set; }
        public string[] Responses { get; set; }
    }
}
