using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ZenGo.Discord.Services;

public class LoggingHandler
{
    public LoggingHandler(IServiceProvider provider)
    {
        var client = provider.GetRequiredService<DiscordSocketClient>();

        var command = provider.GetRequiredService<CommandService>();

        client.Log += OnLogging;

        command.Log += OnLogging;
    }

    private Task OnLogging(LogMessage log)
    {
        switch (log.Severity)
        {
            case LogSeverity.Info:
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.Message}";

                Console.WriteLine(console);

                break;
            }

            case LogSeverity.Error when !log.Exception.Message.Contains("Missing Permissions"):
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.Exception}";

                Console.WriteLine(console);

                break;
            }

            case LogSeverity.Critical:
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.Exception}";

                Console.WriteLine(console);

                break;
            }

            case LogSeverity.Warning:
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.ToString()}";

                Console.WriteLine(console);

                break;
            }

            /*
            case LogSeverity.Debug:
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.Message}";

                Console.WriteLine(console);
                
                break;
            }
            */

            // default: break;
        }

        return Task.CompletedTask;
    }
}