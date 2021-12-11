namespace ZenGo.Core.Entities.Items;

public interface ISpecialEffect
{
    public bool IsKillable { get; }

    public int Step { get; }
}