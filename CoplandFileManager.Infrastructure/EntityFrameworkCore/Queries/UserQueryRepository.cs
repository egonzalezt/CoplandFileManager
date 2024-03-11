namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.Queries;

using Domain.User;
using Domain.User.Repository;
using DbContext;
using System.Threading.Tasks;

public class UserQueryRepository : IUserQueryRepository
{
    private readonly CoplandFileManagerDbContext _context;

    public UserQueryRepository(CoplandFileManagerDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _context.AddAsync(user, cancellationToken);
    }
}
