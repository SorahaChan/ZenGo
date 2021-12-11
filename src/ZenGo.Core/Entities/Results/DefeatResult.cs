namespace ZenGo.Core.Entities.Results;

public class DefeatResult: IProcessResult
{
    internal DefeatResult(string message, string battleResult, string nextMessage, string imageUrl)
    {
        this.Message = message;
        this.BattleResult = battleResult;
        this.NextMessage = nextMessage;
        this.ImageUrl = imageUrl;
    }
    
    public string Message { get; }
    
    public string BattleResult { get; }
    
    public string NextMessage { get; }
    
    public string ImageUrl { get; }
}