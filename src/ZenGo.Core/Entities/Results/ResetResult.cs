namespace ZenGo.Core.Entities.Results;

public class ResetResult: IProcessResult
{
    public ResetResult(string message, string nextMessage, string imageUrl)
    {
        this.Message = message;
        this.NextMessage = nextMessage;
        this.ImageUrl = imageUrl;
    }
    
    public string Message { get; }
    
    public string NextMessage { get; }
    
    public string ImageUrl { get; }
}