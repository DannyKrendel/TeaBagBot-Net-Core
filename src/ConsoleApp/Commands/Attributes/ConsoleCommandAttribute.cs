using System;

namespace TeaBagBot.ConsoleApp.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConsoleCommandAttribute : Attribute
    {
        public string Name { get; }
        public bool IgnoreExtraArgs { get; }

        public ConsoleCommandAttribute()
        {
        }

        public ConsoleCommandAttribute(string name, bool ignoreExtraArgs)
        {
            Name = name;
            IgnoreExtraArgs = ignoreExtraArgs;
        }
    }
}
