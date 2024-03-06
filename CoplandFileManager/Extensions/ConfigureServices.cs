namespace CoplandFileManager.Extensions;

using Application.Services.Configuration;
using Infrastructure.Services.ServiceCollection;

public static class ConfigureServices
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSwagger();
        services.AddApplication();
        services.AddInfrastructure(configuration);
    }
}
