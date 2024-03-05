namespace CoplandFileManager.Extensions;

public static class ConfigureServices
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSwagger();
    }
}
