namespace CoplandFileManager.Domain.File;

using System.Text.Json.Serialization;

public class File
{
    private File(Category category, string name, string format, string objectRoute)
    {
        Name = name;
        Format = format;
        Id = Guid.NewGuid();
        ObjectRoute = objectRoute;
        Category = category;
    }
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Format { get; private set; }
    [JsonIgnore]
    public string ObjectRoute { get; private set; }
    public Category Category { get; private set; }
    public DateTime UploadTime { get; } = DateTime.UtcNow;
    public ICollection<UserFilePermission> UserPermissions { get; private set; }

    public static File Build(Category category, string name, string nameWithExtension, Guid userId)
    {
        var extension = Path.GetExtension(nameWithExtension);
        var objectRoute = $"{userId}/{nameWithExtension}";
        return new(category, name, extension, objectRoute);
    }

    public static string GenerateObjectRoute(string nameWithExtension,  Guid userId)
    {
        return $"{userId}/{nameWithExtension}";
    }
}
