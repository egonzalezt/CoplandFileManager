namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.Commands;

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

    public async Task<Guid?> GetIdByIdentityProviderId(string identityProviderId)
    {
        var result = await _context.Users.Where(u => u.IdentityProviderUserId.Equals(identityProviderId))
            .Select(usr => usr.Id).SingleOrDefaultAsync();
        if (result == Guid.Empty)
        {
            return null;
        }
        return result;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByIdentityProviderIdAsync(string identityProviderUserId)
    {
        return await _context.Users.AnyAsync(u => u.IdentityProviderUserId == identityProviderUserId);
    }

    public async Task<bool> IsActive(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user != null && user.IsActive;
    }

    public async Task<bool> IsActive(string identityProviderUserId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityProviderUserId == identityProviderUserId);
        return user != null && user.IsActive;
    }
}