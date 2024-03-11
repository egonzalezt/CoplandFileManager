namespace CoplandFileManager.Extensions;

using Application.Services.Configuration;
using Frieren_Guard.Extensions;
using Infrastructure.Services.ServiceCollection;
using Workers.Extensions;

public static class ConfigureServices
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSwagger();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddWorkers(configuration);
        services.AddFrierenGuardServices(configuration);
    }
}
