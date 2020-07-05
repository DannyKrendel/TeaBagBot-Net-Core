using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;
using TeaBagBot.Services;

namespace TeaBagBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TeaBagCommandAttribute : CommandAttribute
    {
        public TeaBagCommandAttribute([CallerMemberName] string memberName = "") 
            : base(TeaBagCommandService.GetCommand<MongoRepository<TeaBagCommand>>(memberName).Name)
        {

        }
    }

}
