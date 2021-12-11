namespace ZenGo.Core.Entities.Items;

public class Elixir: IItem
{
    public string DisplayName { get; } = "elixir";

    public int Id { get; } = 3;

    public IReadOnlyList<string> Prefix { get; } = new string[] {"e", "elixir"};
    
    public bool IsDecrease { get; } = true;
}