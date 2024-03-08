namespace CoplandFileManager.Domain.File.Dtos;

public class FileDto
{
    public string NameWithExtension { get; set; }
    public string MimeType { get; set; }
    public Category category { get; set; }
}
