namespace CoplandFileManager.Domain.File;

public class File
{
    private File(Guid userId, string name, string format, string objectId, string objectRoute)
    {
        Name = name;
        Format = format;
        ObjectId = objectId;
        Id = Guid.NewGuid();
        UserId = userId;
        ObjectRoute = objectRoute;
    }
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Format { get; private set; }
    public string ObjectId { get; private set; }
    public string ObjectRoute { get; private set; }
    public DateTime UploadTime { get; } = DateTime.UtcNow;

    public static File Build(Guid userId, string name, string format, string objectId, string objectRoute)
    {
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(name);
        return new(userId, nameWithoutExtension, format, objectId, objectRoute);
    }

}
