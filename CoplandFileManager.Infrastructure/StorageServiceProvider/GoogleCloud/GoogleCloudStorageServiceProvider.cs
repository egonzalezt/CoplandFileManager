namespace CoplandFileManager.Infrastructure.StorageServiceProvider.GoogleCloud;

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using Domain.StorageServiceProvider;
using Services.Options;
using CoplandFileManager.Domain.User;

internal class GoogleCloudStorageServiceProvider : IStorageServiceProvider
{
    private readonly StorageClient _storageClient;
    private readonly ILogger<GoogleCloudStorageServiceProvider> _logger;
    private readonly GoogleCloudStorageOptions _googleCloudStorageOptions;
    private readonly GoogleCredential _googleCredential;
    public GoogleCloudStorageServiceProvider(
        StorageClient storageClient,
        ILogger<GoogleCloudStorageServiceProvider> logger,
        IOptions<GoogleCloudStorageOptions> storageOptions,
        GoogleCredential googleCredential
    )
    {
        _storageClient = storageClient;
        _logger = logger;
        _googleCloudStorageOptions = storageOptions.Value;
        _googleCredential = googleCredential;
    }

    public async Task<string> UploadFileAsync(Stream stream, string objectRoute, string contentType)
    {
        _logger.LogInformation("Uploading file");
        var result = await _storageClient.UploadObjectAsync(
                        bucket: _googleCloudStorageOptions.BucketName,
                        objectName: objectRoute,
                        contentType: contentType,
                        source: stream);
        _logger.LogInformation("File successfully Uploaded with ObjectId: {id}", result.Id);
        return result.Id;
    }

    public async Task<string> GeneratePreSignedUrlAsync(string fileName, Guid userId, TimeSpan expiration)
    {
        _logger.LogInformation("Generating pre-signed URL for object: {id}", fileName);
        var objectRoute = $"{userId}/{fileName}";
        UrlSigner urlSigner = UrlSigner.FromCredential(_googleCredential);
        string url = await urlSigner.SignAsync(_googleCloudStorageOptions.BucketName, objectRoute, expiration);
        _logger.LogInformation("Pre-signed URL generated successfully for object: {id}", fileName);
        return url;
    }

    public async Task<string> GeneratePreSignedUrlForUploadAsync(string fileName, Guid userId, TimeSpan expiration)
    {
        var objectRoute = $"{userId}/{fileName}";
        _logger.LogInformation("Generating pre-signed URL for object: {id}", fileName);
        UrlSigner urlSigner = UrlSigner.FromCredential(_googleCredential);
        string url = await urlSigner.SignAsync(_googleCloudStorageOptions.BucketName, objectRoute, expiration, HttpMethod.Put); 
        _logger.LogInformation("Pre-signed URL generated successfully for object: {id}", fileName);
        return url;
    }
}
