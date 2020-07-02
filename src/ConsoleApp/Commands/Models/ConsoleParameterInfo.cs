using System;
using System.Threading.Tasks;

namespace TeaBagBot.ConsoleApp.Commands.Models
{
    public class ConsoleParameterInfo
    {
        public string Name { get; }
        public Type Type { get; }
        public bool IsOptional { get; }
        public bool IsRemainder { get; }
        public object DefaultValue { get; }

        internal ConsoleParameterInfo(string name, Type type, bool isOptional, bool isRemainder = false, object defaultValue = null)
        {
            Name = name;
            IsOptional = isOptional;
            IsRemainder = isRemainder;
            Type = type;
            DefaultValue = defaultValue;
        }

        public async Task<object> ParseAsync(IConsoleCommandContext context, string input)
        {
            object obj;
            try
            {
                obj = Convert.ChangeType(input, Type);
            }
            catch (Exception)
            {
                return null;
            }
            return obj;
        }

        public override string ToString() => Name;
    }
}
