namespace CoplandFileManager.Infrastructure.Services.ServiceCollection;

using Options;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using EntityFrameworkCore;
using StorageServiceProvider.GoogleCloud;
using Domain.StorageServiceProvider;

public static class InfrastructureServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // GoogleCloud Storage
        var googleCloudOptions = new GoogleCloudAuthenticationOptions();
        configuration.GetSection("GoogleCloud:Authentication").Bind(googleCloudOptions);
        var credentials = GoogleCredential.FromJson(JsonSerializer.Serialize(googleCloudOptions));
        var storageClient = StorageClient.Create(credentials);
        services.AddSingleton(storageClient);
        services.AddSingleton(credentials);
        services.Configure<GoogleCloudStorageOptions>(configuration.GetSection("GoogleCloud:Storage"));
        services.AddSingleton<IStorageServiceProvider, GoogleCloudStorageServiceProvider>();
        // EntityFramework
        services.AddEntityFramework(configuration);
        services.AddRepositories();
    }
}