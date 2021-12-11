namespace ZenGo.Core.Entities.Results;

public class InquiryResult: IProcessResult
{
    public InquiryResult(string message, string imageUrl)
    {
        this.Message = message;
        this.ImageUrl = imageUrl;
    }
    
    public string Message { get; }
    
    public string ImageUrl { get; }
}