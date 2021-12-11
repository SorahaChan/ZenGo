namespace ZenGo.Core.Entities.Results;

public class RankingResult: IProcessResult
{
    public RankingResult(string message)
    {
        this.Message = message;
    }
    
    public string Message { get; }
}