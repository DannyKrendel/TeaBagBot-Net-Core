namespace DiscordBot.Core.Entities
{
    public class CommandEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public ArgEntity Args { get; set; }
        public string[] Responses { get; set; }
        public ulong Permissions { get; set; }
    }

    public class ArgEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
