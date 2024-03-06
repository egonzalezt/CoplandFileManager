namespace CoplandFileManager.Domain.File.Dtos;

public class FileDto
{
    public string IdentityProviderUserId { get; set; }
    public string Name { get; set; }
    public string Format { get; set; }
    public string ObjectId { get; set; }
    public string ObjectRoute {  get; set; }
    public string MimeType { get; set; }
}
