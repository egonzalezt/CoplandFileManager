namespace CoplandFileManager.Responses;

public class BaseResponse<T> where T : class
{
    public string Message { get; set; }
    public T Content { get; set; }
}
