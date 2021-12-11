using ZenGo.Discord.Base;

namespace ZenGo.Discord
{
    public static class Program
    {
        internal static async Task Main(string[] args)
        {
            await RunAsync();
        }

        private static async Task RunAsync()
        {
            await new BotClient().LoginAsync();
        
            await Task.Delay(-1);
        }
    }
}