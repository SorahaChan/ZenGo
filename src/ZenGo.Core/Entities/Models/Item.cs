using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZenGo.Core.Entities.Models;

[Table("items"), Index(nameof(UserId))]
public class Item
{
    public Item(string id, ulong userId, int itemId, int quantity)
    {
        this.Id = id;
        this.UserId = userId;
        this.ItemId = itemId;
        this.Quantity = quantity;
    }
    
    [Required, Key, Column("id", TypeName = "char(42)")]
    public string Id { get; set; }
    
    [Required, Column("user_id")]
    public ulong UserId { get; set; }
    
    [Required, Column("item_id")]
    public int ItemId { get; set; }
    
    [Required, Column("quantity")]
    public int Quantity { get; set; }
}