namespace CoplandFileManager.Domain.File.Dtos;

public class FileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Format { get; set; }
    public Category Category { get; set; }
    public DateTime UploadTime { get; set; }
    public UserFilePermission UserPermissions { get; set; }

}
