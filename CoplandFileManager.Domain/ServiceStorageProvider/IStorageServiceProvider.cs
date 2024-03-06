namespace CoplandFileManager.Domain.StorageServiceProvider;

public interface IStorageServiceProvider
{
    Task<(string objectId, string objectRoute)> UploadFileAsync(Stream stream, string identityProviderId, string fileName, string contentType);
    Task<(string objectId, string objectRoute)> UploadFileAsync(Stream stream, string fileName, string contentType);
    Task<string> GeneratePreSignedUrlAsync(string objectId, TimeSpan expiration);
}
