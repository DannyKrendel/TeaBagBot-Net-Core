using DiscordBot.Core.Attributes;
using DiscordBot.Core.Modules;
using System;
using System.Reflection;

namespace DiscordBot.Core
{
    public static class AttributeUtils
    {
        public static void TryLoadAttributes()
        {
            var modules = new Type[] {
                typeof(StandardModule),
                typeof(AdminModule)
            };
            var attrTypes = new Type[] {
                typeof(CustomAliasAttribute),
                typeof(CustomCommandAttribute)
            };

            foreach (var module in modules)
            {
                foreach (var method in module.GetMethods(BindingFlags.Public))
                {
                    foreach (var type in attrTypes)
                    {
                        if (!Attribute.IsDefined(method, type))
                        {
                            throw new Exception($"Attribute '{type}' was not found on '{method}'.");
                        }
                    }
                }
            }
        }
    }
}
