namespace CoplandFileManager.Domain.File.Dtos;

public class FileUploadDto
{
    public string NameWithExtension { get; set; }
    public string MimeType { get; set; }
    public Category category { get; set; }
}
