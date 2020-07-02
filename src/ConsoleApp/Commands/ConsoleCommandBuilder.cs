using TeaBagBot.ConsoleApp.Commands.Attributes;
using TeaBagBot.ConsoleApp.Commands.Models;
using TeaBagBot.ConsoleApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TeaBagBot.ConsoleApp.Modules;
using TeaBagBot.ConsoleApp.Services;

namespace TeaBagBot.ConsoleApp.Commands
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
                    service._logger.Warning("CmdBuilder: {err}", $"Class {typeInfo.FullName} is not public and cannot be loaded.");
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
            var attributes = method.GetCustomAttributes();
            var commandAttr = attributes.FirstOrDefault(a => a.GetType() == typeof(ConsoleCommandAttribute)) as ConsoleCommandAttribute;
            var aliasAttr = attributes.FirstOrDefault(a => a.GetType() == typeof(ConsoleAliasAttribute)) as ConsoleAliasAttribute;
            var summaryAttr = attributes.FirstOrDefault(a => a.GetType() == typeof(ConsoleSummaryAttribute)) as ConsoleSummaryAttribute;

            if (commandAttr == null)
            {
                return null;
            }

            var parameters = method.GetParameters().Select(p => BuildParameter(p)).ToArray();

            var createInstance = ReflectionUtils.CreateBuilder<ConsoleModuleBase>(typeInfo, service);

            async Task ExecuteCallback(IConsoleCommandContext context, object[] args, IServiceProvider serviceProvider, ConsoleCommandInfo cmd)
            {
                var argsList = args.ToList();
                if (argsList.Count < method.GetParameters().Length)
                {
                    for (int i = argsList.Count; i < method.GetParameters().Length; i++)
                    {
                        argsList.Add(Type.Missing);
                    }
                }

                var module = createInstance(services);

                module.Context = context;

                var task = method.Invoke(module, argsList.ToArray()) as Task ?? Task.Delay(0);

                await task.ConfigureAwait(false);
            }

            return new ConsoleCommandInfo(commandAttr.Name, summaryAttr?.Summary, aliasAttr?.Aliases, 
                parameters, service.Parser, commandAttr.IgnoreExtraArgs, ExecuteCallback);
        }

        public ConsoleParameterInfo BuildParameter(ParameterInfo paramInfo)
        {
            var remainderAttr = paramInfo.GetCustomAttribute<ConsoleRemainderAttribute>();

            return new ConsoleParameterInfo(paramInfo.Name, paramInfo.ParameterType, paramInfo.IsOptional,
                remainderAttr != null, paramInfo.HasDefaultValue ? paramInfo.DefaultValue : null);
        }
    }
}
