﻿namespace CoplandFileManager.Domain.File.Repositories;

public interface IFileCommandRepository
{
    Task<List<File>> GetFilesByUserIdAsync(Guid userId, int pageIndex, int pageSize);
    Task<File?> GetFileByNameAsync(string fileName);
    Task<bool> FileExistsByNameAsync(Guid userId, string fileName);
}