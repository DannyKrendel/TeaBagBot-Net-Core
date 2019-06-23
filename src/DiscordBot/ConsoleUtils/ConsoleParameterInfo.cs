using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.ConsoleUtils
{
    public class ConsoleParameterInfo
    {
        public string Name { get; }
        public string Summary { get; }
        public bool IsOptional { get; }
        public object DefaultValue { get; }
        public bool IsRemainder { get; }
        public Type Type { get; }

        internal ConsoleParameterInfo(string name, Type type, string summary = null, bool isOptional = false, bool isRemainder = false, object defaultValue = null)
        {
            Name = name;
            Summary = summary;
            IsOptional = isOptional;
            IsRemainder = isRemainder;
            Type = type;
            DefaultValue = defaultValue;
        }

        public override string ToString() => Name;
    }
}
