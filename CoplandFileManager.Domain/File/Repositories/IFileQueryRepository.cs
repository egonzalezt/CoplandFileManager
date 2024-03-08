namespace CoplandFileManager.Domain.File.Repositories;

public interface IFileQueryRepository
{
    Task CreateAsync(File file, Guid userId);
}
