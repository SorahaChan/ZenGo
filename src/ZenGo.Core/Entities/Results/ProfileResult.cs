namespace ZenGo.Core.Entities.Results;

public class ProfileResult: IProcessResult
{
    public ProfileResult(string message)
    {
        this.Message = message;
    } 
    
    public string Message { get; }
}