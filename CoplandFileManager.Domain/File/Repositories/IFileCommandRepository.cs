using CoplandFileManager.Domain.File.Dtos;

namespace CoplandFileManager.Domain.File.Repositories;

public interface IFileCommandRepository
{
    Task<PaginationResult<FileDto>> GetFilesByUserIdAsync(Guid userId, int pageIndex, int pageSize);
    Task<File?> GetFileByNameAsync(string fileName);
    Task<bool> FileExistsByNameAsync(Guid userId, string fileName);
    Task<File?> GetFileByIdAndUserIdAsync(Guid fileId, Guid userId);
}