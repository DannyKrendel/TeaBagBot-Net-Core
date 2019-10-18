namespace TeaBagBot.Core.Entities
{
    public class CommandData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public ArgData Args { get; set; }
        public string[] Responses { get; set; }
    }

    public class ArgData
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
