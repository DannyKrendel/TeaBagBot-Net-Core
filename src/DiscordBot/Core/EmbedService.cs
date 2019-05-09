using Discord;
using System;

namespace DiscordBot.Core
{
    public class EmbedService
    {
        public Embed GetInfoEmbed(string title, string description,
            Action<EmbedFooterBuilder> footerBuilder = null,
            EmbedFieldBuilder[] fieldBuilders = null,
            Action<EmbedAuthorBuilder> authorBuilder = null)
        {
            var embedBuilder = new EmbedBuilder();

            embedBuilder
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(new Color(0, 255, 100));

            if (footerBuilder != null)
                embedBuilder.WithFooter(footerBuilder);
            if (fieldBuilders != null)
                embedBuilder.WithFields(fieldBuilders);
            if (authorBuilder != null)
                embedBuilder.WithAuthor(authorBuilder);

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
