using System;
using System.Reflection;

namespace DiscordBot.Utils
{
    public static class AttributeUtils
    {
        public static void TryLoadAttributes(Type[] classes, Type[] attributes)
        {
            foreach (var obj in classes)
            {
                foreach (var method in obj.GetMethods(BindingFlags.Public))
                {
                    foreach (var type in attributes)
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
