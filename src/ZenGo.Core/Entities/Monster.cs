using Newtonsoft.Json;

namespace ZenGo.Core.Entities;

internal struct Monster
{
    [JsonProperty("monster_name")]
    internal string Name { get; set; }
    
    [JsonProperty("monster_id")]
    internal int Id { get; set; }
    
    [JsonProperty("monster_rare")]
    internal MonsterRare Rare { get; set; }
    
    [JsonProperty("monster_image")]
    internal string ImageUrl { get; set; }
}

internal enum MonsterRare
{
    Normal = 1,
    Strong = 2,
    Rare = 4,
    Unknown = 5,
    
}