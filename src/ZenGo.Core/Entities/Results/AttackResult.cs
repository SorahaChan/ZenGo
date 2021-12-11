namespace ZenGo.Core.Entities.Results;

public class AttackResult: IProcessResult
{
    internal AttackResult(string message)
    {
        this.Message = message;
    }
    
    public string Message { get; }
}