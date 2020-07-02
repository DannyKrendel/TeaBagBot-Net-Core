using System;

namespace TeaBagBot.ConsoleApp.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConsoleSummaryAttribute : Attribute
    {
        public string Summary { get; }

        public ConsoleSummaryAttribute()
        {
        }

        public ConsoleSummaryAttribute(string summary)
        {
            Summary = summary;
        }
    }
}
