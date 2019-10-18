﻿using System.Threading.Tasks;

namespace TeaBagBot.Core.Commands
{
    public interface ICommandHandler<T>
    {
        Task HandleMessageAsync(T msg);
        Task InitializeAsync();
    }
}