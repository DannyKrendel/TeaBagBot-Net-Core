using Discord;
using System;

namespace TeaBagBot.Services
{
    public class EmbedService
    {
        private Embed GetEmbed(Color color, string title = null, string description = null, 
            string url = null, string imageUrl = null, string thumbnailUrl = null,
            Action<EmbedFooterBuilder> footerBuilder = null,
            EmbedFieldBuilder[] fieldBuilders = null,
            Action<EmbedAuthorBuilder> authorBuilder = null)
        {
            var embedBuilder = new EmbedBuilder();

            embedBuilder
                .WithTitle(title)
                .WithDescription(description)
                .WithUrl(url)
                .WithImageUrl(imageUrl)
                .WithThumbnailUrl(thumbnailUrl)
                .WithColor(color);

            if (footerBuilder != null)
                embedBuilder.WithFooter(footerBuilder);
            if (fieldBuilders != null)
                embedBuilder.WithFields(fieldBuilders);
            if (authorBuilder != null)
                embedBuilder.WithAuthor(authorBuilder);

            return embedBuilder.Build();
        }

        public Embed GetInfoEmbed(string title, string description = null, 
            string url = null, string imageUrl = null, string thumbnailUrl = null,
            Action<EmbedFooterBuilder> footerBuilder = null,
            EmbedFieldBuilder[] fieldBuilders = null,
            Action<EmbedAuthorBuilder> authorBuilder = null)
        {
            return GetEmbed(new Color(0, 255, 100), title, description, url, imageUrl, thumbnailUrl, footerBuilder, fieldBuilders, authorBuilder);
        }

        public Embed GetErrorEmbed(string title, string description = null, 
            string url = null, string imageUrl = null, string thumbnailUrl = null,
            Action<EmbedFooterBuilder> footerBuilder = null,
            EmbedFieldBuilder[] fieldBuilders = null,
            Action<EmbedAuthorBuilder> authorBuilder = null)
        {
            return GetEmbed(new Color(255, 0, 0), title, description, url, imageUrl, thumbnailUrl, footerBuilder, fieldBuilders, authorBuilder);
        }

        public Embed GetWarningEmbed(string title, string description = null, 
            string url = null, string imageUrl = null, string thumbnailUrl = null,
            Action<EmbedFooterBuilder> footerBuilder = null,
            EmbedFieldBuilder[] fieldBuilders = null,
            Action<EmbedAuthorBuilder> authorBuilder = null)
        {
            return GetEmbed(new Color(255, 255, 0), title, description, url, imageUrl, thumbnailUrl, footerBuilder, fieldBuilders, authorBuilder);
        }
    }
}
