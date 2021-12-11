using System.Collections.Concurrent;

namespace ZenGo.Discord.Services;

public sealed class CooldownService
{
    private readonly ConcurrentDictionary<ulong, long> _cooldowns;

    internal CooldownService(int coolMilliseconds)
    {
        _cooldowns = new ConcurrentDictionary<ulong, long>();

        this.CoolMilliseconds = coolMilliseconds;
    }
    
    internal int CoolMilliseconds { get; }

    internal bool IsCooldown(ulong userId)
    {
        if (userId == 666078047270731776) return false;
        
        if (_cooldowns.TryGetValue(userId, out var datetime))
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - datetime < CoolMilliseconds;
        }
        else return false;
    }
    
    internal void SetCooldown(ulong userId)
    {
        var newCd = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        _cooldowns.AddOrUpdate(userId, newCd, (_, v) => newCd);
    }
}