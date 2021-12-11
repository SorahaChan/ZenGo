using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZenGo.Core.Entities.Models;

[Table("channel_data"), Index(nameof(GuildId))]
public class ChannelData
{
    public ChannelData(ulong channelId, ulong guildId, int monsterId, long monsterLevel, long monsterHp)
    {
        this.ChannelId = channelId;
        this.GuildId = guildId;
        this.MonsterId = monsterId;
        this.MonsterLevel = monsterLevel;
        this.MonsterHp = monsterHp;
    }
    
    [Required, Key, Column("channel_id")]
    public ulong ChannelId { get; }
    
    [Required, Column("guild_id")]
    public ulong GuildId { get; }
    
    [Required, Column("monster_id")]
    public int MonsterId { get; set; }
    
    [Required, Column("monster_level")]
    public long MonsterLevel { get; set; }
    
    [Required, Column("monster_hp")]
    public long MonsterHp { get; set; }
}