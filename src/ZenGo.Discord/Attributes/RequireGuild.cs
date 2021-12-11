using Discord;
using Discord.Commands;

namespace ZenGo.Discord.Attributes;

public class RequireGuild: PreconditionAttribute
{
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        if (context.Channel is ITextChannel) return Task.FromResult(PreconditionResult.FromSuccess());
        
        return Task.FromResult(PreconditionResult.FromError($"`{command.Name}` command is guild-only."));
    }
}