using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.ConsoleApp.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ConsoleRemainderAttribute : Attribute
    {
    }
}
