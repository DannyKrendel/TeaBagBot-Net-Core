using System;

namespace TeaBagBot.ConsoleApp.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConsoleAliasAttribute : Attribute
    {
        public string[] Aliases { get; }

        public ConsoleAliasAttribute()
        {
        }

        public ConsoleAliasAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}
