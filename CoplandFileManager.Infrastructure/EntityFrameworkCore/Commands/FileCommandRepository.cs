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
        return await _context.Files
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.UploadTime)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<File?> GetFileByNameAsync(string fileName)
    {
        return await _context.Files.FirstOrDefaultAsync(f => f.Name == fileName);
    }

    public async Task<bool> FileExistsByNameAsync(Guid userId, string fileName)
    {
        return await _context.Files.AnyAsync(f => f.Name == fileName && f.UserId == userId);
    }
}
