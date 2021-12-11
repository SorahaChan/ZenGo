using Microsoft.EntityFrameworkCore;
using ZenGo.Core.Entities;
using ZenGo.Core.Entities.Models;

namespace ZenGo.Core;

internal class DatabaseContext : DbContext
{
    private readonly string _mariaDbConnection;
        
    internal DatabaseContext(string mariaDbConnection)
    {
        this._mariaDbConnection = mariaDbConnection;
    }

    public DbSet<Player> Players { get; internal set; }
    
    public DbSet<Item> Items { get; internal set; }
    
    public DbSet<Weapon> Weapons { get; internal set; }
    
    public DbSet<Armor> Armors { get; internal set; }
    
    public DbSet<BattleData> BattleDataSet { get; internal set; }
    
    public DbSet<ChannelData> ChannelDataSet { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_mariaDbConnection, new MariaDbServerVersion(new Version(10, 6, 5)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        modelBuilder.Entity<Player>().HasKey(e => new {e.UserId});
            
        modelBuilder.Entity<Item>().HasIndex(e => new {e.UserId});

        modelBuilder.Entity<Weapon>().HasKey(e => new {e.Index});
        
        // modelBuilder.Entity<Weapon>().Property(x => x.Index).ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<Weapon>().HasIndex(e => new {e.UserId});
            
        modelBuilder.Entity<Armor>().HasKey(e => new {e.Index});
        
        // modelBuilder.Entity<Armor>().Property(x => x.Index).ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<Armor>().HasIndex(e => new {e.UserId});
        
        modelBuilder.Entity<BattleData>().HasKey(e => new {UserId = e.UserId});
        
        modelBuilder.Entity<BattleData>().HasIndex(e => new {e.ChannelId});
        
        modelBuilder.Entity<ChannelData>().HasKey(e => new {e.ChannelId});
        
        modelBuilder.Entity<ChannelData>().HasIndex(e => new {e.GuildId});
    }
}