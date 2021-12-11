namespace ZenGo.Core.Entities.Items;

public class FireBook: IMagic
{
    public string DisplayName { get; } = "FireBook";

    public int Id { get; } = 2;

    public IReadOnlyList<string> Prefix { get; } = new string[] {"f", "fb", "firebook"};

    public bool IsDecrease { get; } = true;

    public double MagicEffect { get; } = 1.0;
}