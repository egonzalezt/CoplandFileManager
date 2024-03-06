namespace CoplandFileManager.Infrastructure.EntityFrameworkCore;

using Domain.User.Repository;
using Commands;
using DbContext;
using Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CoplandFileManager.Domain.File.Repositories;

internal static class ConfigureEntityFramework
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CoplandFileManagerDbContext>(options => {
            options.UseNpgsql(configuration.GetConnectionString("PostgresSql"));
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserCommandRepository, UserCommandRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        services.AddScoped<IFileCommandRepository, FileCommandRepository>();
        services.AddScoped<IFileQueryRepository, FileQueryRepository>();
    }
}
