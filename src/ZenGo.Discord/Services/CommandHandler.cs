using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ZenGo.Discord.Base;

namespace ZenGo.Discord.Services;

public sealed class CommandHandler
{
    private readonly DiscordSocketClient _client;

    private readonly CommandService _command;

    private readonly IServiceProvider _provider;

    public CommandHandler(IServiceProvider provider)
    {
        _client = provider.GetRequiredService<DiscordSocketClient>();

        _command = provider.GetRequiredService<CommandService>();
            
        _provider = provider;
    }

    public async Task InitializeAsync()
    {
        await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        
        _client.MessageReceived += OnMessageAsync;
    }
        
    private async Task OnMessageAsync(SocketMessage socketMessage)
    {
        if (socketMessage is SocketUserMessage message)
        {
            if (message.Author.IsBot || message.Author.Id == _client.CurrentUser.Id) return;
                
            var context = new SocketCommandContext(_client, message);
            var argPos = 0;

            if (message.HasStringPrefix(BotInformation.CommandPrefix, ref argPos))
            {
                if (message.Channel is IPrivateChannel or IGroupChannel)
                {
                    await message.Channel.SendMessageAsync("All commands are not available in DM.");
                    
                    return;
                }
                    
                var result = await _command.ExecuteAsync(context, argPos, _provider);
                if (!result.IsSuccess)
                {
                    await context.Message.ReplyAsync("**ERROR**\n\n" + result.ErrorReason, allowedMentions: AllowedMentions.None);
                }
            }
        }
    }
}