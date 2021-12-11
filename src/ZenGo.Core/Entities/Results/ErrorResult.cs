namespace ZenGo.Core.Entities.Results;

public class ErrorResult: IProcessResult
{
    internal ErrorResult(string error)
    {
        this.Message = error;
    }
    
    public string Message { get; }
}