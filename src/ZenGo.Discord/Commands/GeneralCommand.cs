using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ZenGo.Discord.Helpers;

namespace ZenGo.Discord.Commands;

public class GeneralCommand: ModuleBase<SocketCommandContext>
{
    private readonly DiscordSocketClient _client;
    
    public GeneralCommand(DiscordSocketClient client)
    {
        _client = client;
    }
    
    [Command("ping")]
    public async Task PingAsync()
    {
        IUserMessage message = await Context.Message.ReplyAsync(embed: Converter.Embed(description: "ping...!"), allowedMentions: AllowedMentions.None);
            
        int messageLatency = (int) (message.Timestamp - Context.Message.CreatedAt).TotalMilliseconds;
        int wsLatency = _client.Latency;

        await message.ModifyAsync(x => x.Embed = Converter.Embed(description: $"`message`:{messageLatency}ms\n`ws`: {wsLatency}ms"));
    }
}