using Discord;
using ZenGo.Core.Entities;
using ZenGo.Core.Entities.Models;

namespace ZenGo.Core.Extensions;

public static class PlayerExtension
{
    public static int GetLevel(this Player player) => (int) Math.Sqrt(player.Exp);
    
    public static int GetDefaultHp(this Player player) => player.GetLevel() * 5 + 50;
}