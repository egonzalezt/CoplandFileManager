namespace CoplandFileManager.Domain.File;

public class PaginationResult<T>
{
    public List<T> Data { get; set; } = new List<T>();
    public int CurrentPage { get; set; }
    public int NextPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
}
