namespace CoplandFileManager.Domain.User.Repository;

public interface IUserCommandRepository
{
    Task<bool> ExistsByIdAsync(Guid id);
    Task<User?> GetByIdAsync(Guid id);
    Task<bool> IsActive(Guid id);
}
