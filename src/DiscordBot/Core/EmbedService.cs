using Discord;

namespace DiscordBot.Core
{
    public class EmbedService
    {
        public Embed GetInfoEmbed(string title, string description)
        {
            var embedBuilder = new EmbedBuilder();

            embedBuilder
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(new Color(0, 255, 100));

            return embedBuilder.Build();
        }

        public Embed GetErrorEmbed(string title, string description)
        {
            var embedBuilder = new EmbedBuilder();

            embedBuilder
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(new Color(255, 0, 0));

            return embedBuilder.Build();
        }
    }
}
