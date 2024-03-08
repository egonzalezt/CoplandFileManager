namespace CoplandFileManager.Domain.File;

using Domain.User;

public class UserFilePermission
{
    public Guid UserId { get; set; }
    public Guid FileId { get; set; }
    public Permission Permission { get; set; }
    public User User { get; set; }
    public File File { get; set; }
}
