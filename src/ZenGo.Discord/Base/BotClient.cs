using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ZenGo.Core;
using ZenGo.Discord.Services;

namespace ZenGo.Discord.Base;

public class BotClient
{
    private DiscordSocketClient _client;

    private const GatewayIntents Intents = GatewayIntents.Guilds |
                                           GatewayIntents.DirectMessages |
                                           GatewayIntents.GuildMembers |
                                           GatewayIntents.GuildMessages |
                                           GatewayIntents.GuildMessageReactions;

    public async Task LoginAsync()
    {
        BotInformation.Initialize();
            
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = Intents,
            LogLevel = LogSeverity.Debug,
            MessageCacheSize = 1000
        });
            
        var provider = BuildProvider();
            
        provider.GetRequiredService<LoggingHandler>();
            
        await provider.GetRequiredService<CommandHandler>().InitializeAsync();

        _client = provider.GetRequiredService<DiscordSocketClient>();
            
        await _client.LoginAsync(TokenType.Bot, BotInformation.DiscordToken);
        await _client.StartAsync();

        await _client.SetActivityAsync(new Game($"prefix z | ver.{BotInformation.BotVersion}"));
    }

    private IServiceProvider BuildProvider()
    {
        return new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug
            }))
            .AddSingleton(new ZenGoService(new ZenGoServiceConfig()
            {
                DatabaseConnection = BotInformation.DatabaseConnection,
                MonsterJsonPath = AppContext.BaseDirectory + "assets/monster.json"
            }))
            .AddSingleton(new CooldownService(3000))
            .AddSingleton<CommandHandler>()
            .AddSingleton<LoggingHandler>()
            .BuildServiceProvider();
    }
}