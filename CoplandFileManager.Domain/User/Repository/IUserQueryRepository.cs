namespace CoplandFileManager.Domain.User.Repository;

public interface IUserQueryRepository
{
    Task CreateAsync(User user);
}
