namespace TeaBagBot.Core.Entities
{
    public class TeaBagCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
    }
}
