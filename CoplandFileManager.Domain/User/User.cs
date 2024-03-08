namespace CoplandFileManager.Domain.User;

using File;

public class User
{
    private User(Guid id, string identityProviderUserId) 
    { 
        Id = id;
        IdentityProviderUserId = identityProviderUserId;
    }

    public Guid Id { get; private set; }
    public string IdentityProviderUserId { get; private set; }
    public bool IsActive { get; private set; } = false;
    public ICollection<UserFilePermission> UserPermissions { get; private set; }

    public void SetActive()
    {
        IsActive = true;
    }

    public static User CreateUser(string identityProviderUserId)
    {
        var id = Guid.NewGuid();
        var user = new User(id, identityProviderUserId);
        user.SetActive();
        return user;
    }
}
