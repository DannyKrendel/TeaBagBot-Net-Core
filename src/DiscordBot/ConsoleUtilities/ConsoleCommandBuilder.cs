using DiscordBot.ConsoleUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.ConsoleUtilities
{
    internal class ConsoleCommandBuilder
    {
        private readonly ConsoleCommandModule _commandsModule;
        public IReadOnlyList<ConsoleCommandInfo> Commands { get; private set; }

        public ConsoleCommandBuilder()
        {
            _commandsModule = new ConsoleCommandModule();
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

        public void AddCommands(IReadOnlyList<TypeInfo> modules)
        {
            var commands = new List<ConsoleCommandInfo>();

            foreach (var module in modules)
            {
                foreach (var method in module.GetMethods())
                {
                    var command = BuildCommand(typeof(ConsoleCommandModule).GetTypeInfo(), method, null);

                    if (command != null)
                        commands.Add(command);
                }
            }

            Commands = commands.AsReadOnly();
        }

        private ConsoleCommandInfo BuildCommand(TypeInfo typeInfo, MethodInfo method, IServiceProvider serviceprovider)
        {
            string[] parameters;
            var attr = method.GetCustomAttributes().FirstOrDefault(
                a => a.GetType() == typeof(ConsoleCommandAttribute)) as ConsoleCommandAttribute;
            if (attr != null)
            {
                parameters = method.GetParameters().Select(p => p.Name).ToArray();
            }
            else
            {
                return null;
            }

            async Task ExecuteCallback(ConsoleCommandContext context, object[] args, IServiceProvider services, ConsoleCommandInfo cmd)
            {
                _commandsModule.Context = context;

                var task = method.Invoke(_commandsModule, args) as Task ?? Task.Delay(0);

                await task.ConfigureAwait(false);
            }

            return new ConsoleCommandInfo(attr.Name, attr.Summary, attr.Aliases, parameters, ExecuteCallback);
        }
    }
}
