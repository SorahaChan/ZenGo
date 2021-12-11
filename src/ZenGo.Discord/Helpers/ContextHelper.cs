using Discord;
using Discord.Commands;
using ZenGo.Core.Entities.Results;

namespace ZenGo.Discord.Helpers;

internal static class ContextHelper
{
    internal static Task SendResultAsync(this ICommandContext context, IProcessResult result)
    {
       return result switch
        {
            // var e = expMessage.Length <= 2000 ? expMessage : "description length more than 2000.";
            DefeatResult next => context.Channel.SendMessageAsync(embeds: new Embed[]
            {
                Converter.Embed(description: CheckLength(next.Message)),
                Converter.Embed(description: CheckLength(next.BattleResult)),
                Converter.Embed(description: CheckLength(next.NextMessage), thumbnailUrl: next.ImageUrl)
            }),
            
            ResetResult reset => context.Channel.SendMessageAsync(embeds: new Embed[]
            {
                Converter.Embed(description: CheckLength(reset.Message)),
                Converter.Embed(description: CheckLength(reset.NextMessage), thumbnailUrl: reset.ImageUrl)
            }),
            
            InquiryResult inquiry => context.Channel.SendMessageAsync(embeds: new Embed[]
            {
                Converter.Embed(description: CheckLength(inquiry.Message), thumbnailUrl: inquiry.ImageUrl),
            }),
                
            // Include AttackResult, ErrorResult, etc.
            _ => context.Channel.SendMessageAsync(embed: Converter.Embed(description: CheckLength(result.Message))),
        };
    }

    private static string CheckLength(string str)
    {
        return str.Length <= 2000 ? str : "description length more than 2000.";
    }
}