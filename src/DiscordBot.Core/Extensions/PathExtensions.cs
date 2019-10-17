using System.IO;
using System.IO.Abstractions;

namespace DiscordBot.Extensions
{
    public static class PathExtensions
    {
        public static bool IsPathFullyQualified(this IPath thisPath, string path)
        {
            return Path.IsPathFullyQualified(path);
        }
    }
}
