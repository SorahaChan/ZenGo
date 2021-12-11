namespace ZenGo.Core.Entities.Results;

public class ItemResult: IProcessResult
{
    public ItemResult(string message)
    {
        this.Message = message;
    }
    
    public string Message { get; }
}