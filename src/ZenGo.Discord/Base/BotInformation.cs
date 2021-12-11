using Discord;
using Newtonsoft.Json;

namespace ZenGo.Discord.Base;

public static class BotInformation
{
    // base setting file
    private static readonly string ConfigurationPath = AppContext.BaseDirectory + "assets/config.json";
        
    private static JsonData _configuration;
        
    internal static void Initialize()
    {
        _configuration = JsonConvert.DeserializeObject<JsonData>(File.ReadAllText(ConfigurationPath));
    }

    internal static string DiscordToken => _configuration.DiscordToken;
        
    internal static string CommandPrefix => _configuration.CommandPrefix;

    internal static string BotVersion => "1.0.0-alpha.1";

    internal static string DiscordNetVersion => typeof(Embed).Assembly.GetName().Version?.ToString() ?? "None";
        
    internal static string DatabaseConnection=> _configuration.DatabaseConnection;
        
    private struct JsonData
    {
        [JsonProperty("discord_token")]
        internal string DiscordToken { get; set; }
            
        [JsonProperty("command_prefix")]
        internal string CommandPrefix { get; set; }
            
        [JsonProperty("database_connection")]
        internal string DatabaseConnection { get; set; }
    }
}