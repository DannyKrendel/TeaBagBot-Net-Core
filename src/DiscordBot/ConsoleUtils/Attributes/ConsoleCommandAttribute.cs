using System;

namespace DiscordBot.ConsoleUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConsoleCommandAttribute : Attribute
    {
        public string Name { get; }
        public string Summary { get; }
        public string[] Aliases { get; }

        public ConsoleCommandAttribute()
        {
        }

        public ConsoleCommandAttribute(string name, string summary = null, params string[] aliases)
        {
            Name = name;
            Summary = summary;
            Aliases = aliases;
        }
    }
}
