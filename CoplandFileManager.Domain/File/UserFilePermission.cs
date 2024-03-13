namespace CoplandFileManager.Domain.File;

using Domain.User;
using System.Text.Json.Serialization;

public class UserFilePermission
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public Guid FileId { get; set; }
    public Permission Permission { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    [JsonIgnore]
    public File File { get; set; }
}
