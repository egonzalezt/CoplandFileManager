namespace CoplandFileManager.Domain.File.Dtos;

using System.Text.Json.Serialization;

public class FileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Format { get; set; }
    public Category Category { get; set; }
    public DateTime UploadTime { get; set; }
    public UserFilePermission UserPermissions { get; set; }
    [JsonIgnore]
    public string ObjectRoute { get; set; }
}
