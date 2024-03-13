namespace CoplandFileManager.Application.Interfaces;

using CoplandFileManager.Domain.File.Dtos;
using Domain.File;

public interface IGetFilesUseCase
{
    Task<PaginationResult<FileDto>> GetAsync(Guid userId, int page = 1, int pageSize = 10);
}
