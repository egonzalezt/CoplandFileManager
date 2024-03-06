namespace CoplandFileManager.Domain.User.Repository;

public interface IUserCommandRepository
{
    Task<bool> ExistsByIdAsync(Guid id);
    Task<bool> ExistsByIdentityProviderIdAsync(string identityProviderUserId);
    Task<Guid?> GetIdByIdentityProviderId(string identityProviderId);
    Task<bool> IsActive(Guid userId);
    Task<bool> IsActive(string identityProviderUserId);
}
