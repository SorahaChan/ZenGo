using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZenGo.Core.Entities.Models;

[Table("armor"), Index(nameof(UserId))]
public class Armor
{
    public Armor(ulong userId, int armorId)
    {
        this.UserId = userId;
        this.ArmorId = armorId;
        this.Exp = 1;
    }
    
    [Required, Key, Column("index")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Index { get; set; }
    
    [Required, Column("user_id")]
    public ulong UserId { get; set; }
    
    [Required, Column("armor_id")]
    public int ArmorId { get; set; }
    
    [Required, Column("armor_type")]
    public int ArmorType { get; set; }
    
    [Required, Column("exp")]
    public int Exp { get; set; }
    
    [Column("enchant")]
    public int Enchant { get; set; }
}