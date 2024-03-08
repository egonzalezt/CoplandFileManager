namespace CoplandFileManager.Responses;

public class SignedUrlResponse
{
    public string Url { get; set; }
    public TimeSpan TimeLimit { get; set; }
}
