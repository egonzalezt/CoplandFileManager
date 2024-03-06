namespace CoplandFileManager.Application.Services.Configuration;

using Interfaces;
using UseCases;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigureApplication
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateFileUseCase, CreateFileUseCase>();
        services.AddScoped<IGetSignedUrlUseCase, GetSignedUrlUseCase>();
    }
}