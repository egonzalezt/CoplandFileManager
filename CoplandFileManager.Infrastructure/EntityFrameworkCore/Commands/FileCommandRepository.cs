namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.Commands;

using DbContext;
using Domain.File.Repositories;
using Domain.File;
using Microsoft.EntityFrameworkCore;

public class FileCommandRepository : IFileCommandRepository
{
    private readonly CoplandFileManagerDbContext _context;

    public FileCommandRepository(CoplandFileManagerDbContext context)
    {
        _context = context;
    }

    public async Task<List<File>> GetFilesByUserIdAsync(Guid userId, int pageIndex, int pageSize)
    {
        return await (from uf in _context.UserFilePermissions
                      where uf.UserId == userId
                      join f in _context.Files on uf.FileId equals f.Id
                      select f)
                      .Skip(pageIndex * pageSize)
                      .Take(pageSize)
                      .ToListAsync();
    }

    public async Task<File?> GetFileByNameAsync(string fileName)
    {
        return await _context.Files.FirstOrDefaultAsync(f => f.Name == fileName);
    }

    public async Task<bool> FileExistsByNameAsync(Guid userId, string fileName)
    {
        return await _context.UserFilePermissions.AnyAsync(uf =>
            uf.UserId == userId &&
            uf.File.Name == fileName);
    }

    public async Task<File?> GetFileByIdAndUserIdAsync(Guid fileId, Guid userId)
    {
        return await _context.Files
            .Where(f => f.Id == fileId &&
                        f.UserPermissions.Any(p => p.UserId == userId ))
            .FirstOrDefaultAsync();
    }
}
