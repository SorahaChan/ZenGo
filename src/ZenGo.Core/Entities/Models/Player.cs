using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenGo.Core.Entities.Models;

[Table("players")]
public class Player
{
    public Player(ulong userId)
    {
        this.UserId = userId;
        this.Exp = 1;
    }
    
    [Required, Key, Column("user_id")]
    public ulong UserId { get; set; }
    
    [Required, Column("exp")]
    public long Exp { get; set; }
    
    [Column("set_weapon")]
    public int SetWeapon { get; set; }
    
    [Column("set_armor_head")]
    public int SetArmorHead { get; set; }
    
    [Column("set_armor_chest")]
    public int SetArmorChest { get; set; }
    
    [Column("set_armor_boots")]
    public int SetArmorBoots { get; set; }
}