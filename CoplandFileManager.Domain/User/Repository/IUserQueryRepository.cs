using System.Threading;

namespace CoplandFileManager.Domain.User.Repository;

public interface IUserQueryRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken);
}
