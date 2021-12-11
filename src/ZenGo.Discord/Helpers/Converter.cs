using Discord;

namespace ZenGo.Discord.Helpers;

internal static class Converter
{
    private static Color RandomColor()
    {
        var r = new Random();
        return new Color(r.Next(256), r.Next(256), r.Next(256));
    }

    internal static Embed Embed(string title = null, string description = null, Color? color = null,
        EmbedAuthorBuilder author = null, IReadOnlyList<EmbedFieldBuilder> fields = null,
        EmbedFooterBuilder footer = null, string imageUrl = null, string thumbnailUrl = null,
        DateTimeOffset? timestamp = null, string url = null)
    {
        var builder = new EmbedBuilder
        {
            Color = color ?? RandomColor(),
            Description = description,
            Title = title,
        };

        if (author != null) builder.WithAuthor(author);
            
        if (fields?.Count != null && fields.Count != 0)  builder.WithFields(fields);
            
        if (footer != null) builder.WithFooter(footer);
            
        if (imageUrl != null) builder.WithImageUrl(imageUrl);
            
        if (thumbnailUrl != null) builder.WithThumbnailUrl(thumbnailUrl);
            
        if (timestamp != null) builder.WithTimestamp(timestamp.Value);
            
        if (url != null) builder.WithUrl(url);

        return builder.Build();
    }

    internal static string WithSign(this int integer)
    {
        return integer != 0 ? integer.ToString( "+#;-#;" ) : integer.ToString();
    } 
}