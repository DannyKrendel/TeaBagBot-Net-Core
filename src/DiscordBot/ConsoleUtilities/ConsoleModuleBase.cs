using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.ConsoleUtilities
{
    public abstract class ConsoleModuleBase : ConsoleModuleBase<ConsoleCommandContext> { }

    public abstract class ConsoleModuleBase<T> where T : class
    {
        public T Context { get; set; }
    }
}
