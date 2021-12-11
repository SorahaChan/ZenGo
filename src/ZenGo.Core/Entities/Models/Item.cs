using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZenGo.Core.Entities.Models;

[Table("items"), Index(nameof(UserId))]
public class Item
{
    public Item(ulong userId, int itemId, int quantity)
    {
        this.UserId = userId;
        this.ItemId = itemId;
        this.Quantity = quantity;
    }
    
    [Required, Column("user_id")]
    public ulong UserId { get; set; }
    
    [Required, Column("item_id")]
    public int ItemId { get; set; }
    
    [Required, Column("quantity")]
    public int Quantity { get; set; }
}