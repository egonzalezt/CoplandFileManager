namespace CoplandFileManager.Domain.StorageServiceProvider;

public interface IStorageServiceProvider
{
    Task<string> UploadFileAsync(Stream stream, string objectRoute, string contentType);
    Task<string> GeneratePreSignedUrlAsync(string fileName, Guid userId, TimeSpan expiration);
    Task<string> GeneratePreSignedUrlForUploadAsync(string fileName, Guid userId, TimeSpan expiration);
}
