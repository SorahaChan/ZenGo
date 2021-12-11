namespace ZenGo.Core.Entities.Items;

public class NfBook: IMagic, ISpecialEffect
{
    public string DisplayName { get; } = "なぎさの魔法書";

    public int Id { get; } = -1;
    public IReadOnlyList<string> Prefix { get; } = new string[] {"nf", "NfBook"};

    public bool IsDecrease { get; } = false;

    public double MagicEffect { get; } = 1;

    public bool IsKillable { get; } = true;

    public int Step { get; } = 100;
}