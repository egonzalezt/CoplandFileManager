namespace CoplandFileManager.Infrastructure.StorageServiceProvider.GoogleCloud;

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using Domain.StorageServiceProvider;
using Services.Options;

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

    public async Task<(string objectId, string objectRoute)> UploadFileAsync(Stream stream, string identityProviderId, string fileName, string contentType)
    {
        _logger.LogInformation("Uploading file");
        var objectRoute = $"{identityProviderId}/{fileName}";
        var result = await _storageClient.UploadObjectAsync(
                        bucket: _googleCloudStorageOptions.BucketName,
                        objectName: objectRoute,
                        contentType: contentType,
                        source: stream);
        _logger.LogInformation("File successfully Uploaded with ObjectId: {id}", result.Id);
        return (result.Id, objectRoute);
    }

    public async Task<(string objectId, string objectRoute)> UploadFileAsync(Stream stream, string fileName, string contentType)
    {
        _logger.LogInformation("Uploading file");
        var result = await _storageClient.UploadObjectAsync(
                        bucket: _googleCloudStorageOptions.BucketName,
                        objectName: fileName,
                        contentType: contentType,
                        source: stream);
        _logger.LogInformation("File successfully Uploaded with ObjectId: {id}", result.Id);
        return (result.Id, fileName);
    }

    public async Task<string> GeneratePreSignedUrlAsync(string objectId, TimeSpan expiration)
    {
        _logger.LogInformation("Generating pre-signed URL for object: {id}", objectId);

        UrlSigner urlSigner = UrlSigner.FromCredential(_googleCredential);
        string url = await urlSigner.SignAsync(_googleCloudStorageOptions.BucketName, objectId, expiration);
        _logger.LogInformation("Pre-signed URL generated successfully for object: {id}", objectId);
        return url;
    }

    public async Task<string> GeneratePreSignedUrlForUploadAsync(string objectId, string identityProviderId, TimeSpan expiration)
    {
        var objectRoute = $"{identityProviderId}/{objectId}";
        _logger.LogInformation("Generating pre-signed URL for object: {id}", objectId);
        UrlSigner urlSigner = UrlSigner.FromCredential(_googleCredential);
        string url = await urlSigner.SignAsync(_googleCloudStorageOptions.BucketName, objectRoute, expiration, HttpMethod.Put); 
        _logger.LogInformation("Pre-signed URL generated successfully for object: {id}", objectId);
        return url;
    }
}
