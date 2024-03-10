namespace CoplandFileManager.Domain.User;

using File;

public class User
{
    private User(Guid id, string email) 
    { 
        Id = id;
        Email = email;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; } = false;
    public ICollection<UserFilePermission> UserPermissions { get; private set; }

    public void SetActive()
    {
        IsActive = true;
    }

    public static User CreateUser(Guid id, string email)
    {
        var user = new User(id, email);
        user.SetActive();
        return user;
    }
}
