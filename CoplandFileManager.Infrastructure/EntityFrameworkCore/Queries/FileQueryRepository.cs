namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.Queries;

using Domain.File;
using Domain.File.Repositories;
using DbContext;
using System.Threading.Tasks;

public class FileQueryRepository : IFileQueryRepository
{
    private readonly CoplandFileManagerDbContext _context;

    public FileQueryRepository(CoplandFileManagerDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(File file, Guid userId)
    {
        var userFilePermission = new UserFilePermission
        {
            UserId = userId,
            FileId = file.Id,
            Permission = Permission.Owner
        };
        await _context.Files.AddAsync(file);
        await _context.UserFilePermissions.AddAsync(userFilePermission);
        await _context.SaveChangesAsync();
    }
}
