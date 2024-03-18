namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.Commands;

using DbContext;
using Domain.File.Repositories;
using Domain.File;
using Microsoft.EntityFrameworkCore;
using CoplandFileManager.Domain.File.Dtos;

public class FileCommandRepository : IFileCommandRepository
{
    private readonly CoplandFileManagerDbContext _context;

    public FileCommandRepository(CoplandFileManagerDbContext context)
    {
        _context = context;
    }


    public async Task<PaginationResult<FileDto>> GetFilesByUserIdAsync(Guid userId, int pageIndex, int pageSize)
    {
        var query = (from uf in _context.UserFilePermissions
                     where uf.UserId == userId
                     join f in _context.Files.Include(f => f.UserPermissions) on uf.FileId equals f.Id
                     select new FileDto
                     {
                         Id = f.Id,
                         Name = f.Name,
                         Format = f.Format,
                         Category = f.Category,
                         UploadTime = f.UploadTime,
                         UserPermissions = f.UserPermissions.First(up => up.UserId == userId)
                     });

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var result = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

        return new PaginationResult<FileDto>
        {
            Data = result,
            CurrentPage = pageIndex,
            NextPage = pageIndex + 1,
            TotalPages = totalPages,
            HasNextPage = (pageIndex + 1) < totalPages
        };
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

    public async Task<bool> FileExistsByObjectRouteAsync(Guid userId, string objectRoute)
    {
        return await _context.Files.AnyAsync(f =>
            f.UserPermissions.Any(up => up.UserId == userId) &&
            f.ObjectRoute == objectRoute);
    }
}
