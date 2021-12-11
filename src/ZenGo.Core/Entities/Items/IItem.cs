namespace ZenGo.Core.Entities.Items;

public interface IItem
{
    public string DisplayName { get; }
    
    public int Id { get; }
    
    public IReadOnlyList<string> Prefix { get; }
    
    public bool IsDecrease { get; }
}