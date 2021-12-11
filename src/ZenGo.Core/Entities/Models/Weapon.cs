using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZenGo.Core.Entities.Models;

[Table("weapons"), Index(nameof(UserId))]
public class Weapon
{
    public Weapon(ulong userId, int weaponId)
    {
        this.UserId = userId;
        this.WeaponId = weaponId;
        this.Exp = 1;
    }
    
    [Required, Key, Column("index")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Index { get; set; }
    
    [Required, Column("user_id")]
    public ulong UserId { get; set; }
    
    [Required, Column("weapon_id")]
    public int WeaponId { get; set; }
    
    [Required, Column("exp")]
    public int Exp { get; set; }
    
    [Column("enchant_1")]
    public int Enchant1 { get; set; }
    
    [Column("enchant_2")]
    public int Enchant2 { get; set; }
    
    [Column("enchant_3")]
    public int Enchant3 { get; set; }
}