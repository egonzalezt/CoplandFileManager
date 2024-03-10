namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.Commands;

using Domain.User;
using DbContext;
using Domain.User.Repository;
using Microsoft.EntityFrameworkCore;

public class UserCommandRepository : IUserCommandRepository
{
    private readonly CoplandFileManagerDbContext _context;

    public UserCommandRepository(CoplandFileManagerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> IsActive(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        return user != null && user.IsActive;
    }
}