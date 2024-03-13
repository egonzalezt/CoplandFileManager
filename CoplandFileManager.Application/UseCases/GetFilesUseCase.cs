namespace CoplandFileManager.Application.UseCases;

using Application.Interfaces;
using Domain.File.Repositories;
using System;
using System.Threading.Tasks;
using Domain.File.Dtos;
using Domain.File;

public class GetFilesUseCase(IFileCommandRepository fileCommandRepository) : IGetFilesUseCase
{
    public async Task<PaginationResult<FileDto>> GetAsync(Guid userId, int pageIndex = 0, int pageSize = 10)
    {
        var files = await fileCommandRepository.GetFilesByUserIdAsync(userId, pageIndex, pageSize); 
        return files;
    }
}
