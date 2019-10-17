using DiscordBot.Console.Attributes;
using DiscordBot.Console.Entities;
using DiscordBot.Console.Modules;
using DiscordBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Console.Commands
{
    internal class ConsoleCommandBuilder
    {
        public IReadOnlyList<ConsoleCommandInfo> Commands { get; private set; }

        public ConsoleCommandBuilder()
        {
        }

        public static async Task<IReadOnlyList<TypeInfo>> SearchModulesAsync(Assembly assembly, ConsoleCommandService service)
        {
            bool IsLoadableModule(TypeInfo info)
            {
                return info.DeclaredMethods.Any(x => x.GetCustomAttribute<ConsoleCommandAttribute>() != null);
            }

            var result = new List<TypeInfo>();

            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.IsPublic || typeInfo.IsNestedPublic)
                {
                    if (IsValidModuleDefinition(typeInfo))
                    {
                        result.Add(typeInfo);
                    }
                }
                else if (IsLoadableModule(typeInfo))
                {
                    await service._logger.LogWarningAsync("CmdBuilder", $"Class {typeInfo.FullName} is not public and cannot be loaded.").ConfigureAwait(false);
                }
            }

            return result;
        }

        private static bool IsValidModuleDefinition(TypeInfo typeInfo)
        {
            return typeof(ConsoleModuleBase).GetTypeInfo().IsAssignableFrom(typeInfo) &&
                   !typeInfo.IsAbstract &&
                   !typeInfo.ContainsGenericParameters;
        }

        public void AddCommands(IReadOnlyList<TypeInfo> modules, ConsoleCommandService service, IServiceProvider serviceProvider)
        {
            var commands = new List<ConsoleCommandInfo>();

            foreach (var module in modules)
            {
                foreach (var method in module.GetMethods())
                {
                    var command = BuildCommand(typeof(ConsoleModule).GetTypeInfo(), method, service, serviceProvider);

                    if (command != null)
                        commands.Add(command);
                }
            }

            Commands = commands.AsReadOnly();
        }

        private ConsoleCommandInfo BuildCommand(TypeInfo typeInfo, MethodInfo method, ConsoleCommandService service, IServiceProvider services)
        {
            var attr = method.GetCustomAttributes().FirstOrDefault(
                a => a.GetType() == typeof(ConsoleCommandAttribute)) as ConsoleCommandAttribute;

            if (attr == null)
            {
                return null;
            }

            var parameters = method.GetParameters().Select(p => new ConsoleParameterInfo(p.Name, p.ParameterType)).ToArray();

            var createInstance = ReflectionUtils.CreateBuilder<ConsoleModuleBase>(typeInfo, service);

            async Task ExecuteCallback(ConsoleCommandContext context, object[] args, IServiceProvider serviceProvider, ConsoleCommandInfo cmd)
            {
                var module = createInstance(services);

                module.Context = context;

                var task = method.Invoke(module, args) as Task ?? Task.Delay(0);

                await task.ConfigureAwait(false);
            }

            return new ConsoleCommandInfo(attr.Name, attr.Summary, attr.Aliases, parameters, ExecuteCallback);
        }
    }
}
