using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZenGo.Core.Entities.Models;

[Table("battle_data"), Index(nameof(ChannelId))]
public class BattleData
{
    public BattleData(ulong userId, ulong channelId, long userHp)
    {
        this.UserId = userId;
        this.ChannelId = channelId;
        this.UserHp = userHp;
    }
    
    [Required, Key, Column("user_id")]
    public ulong UserId { get; set; }
    
    [Required, Column("channel_id")]
    public ulong ChannelId { get; set; }
    
    [Required, Column("user_hp")]
    public long UserHp { get; set; }
}