namespace CoplandFileManager.Workers.Extensions;

using Workers.MessageBroker.Options;
using Workers.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

public static class ServiceCollectionExtensions
{
    public static void AddWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConnectionFactory>(sp =>
        {
            var factory = new ConnectionFactory();
            configuration.GetSection("RabbitMQ:Connection").Bind(factory);
            return factory;
        });
        services.Configure<ConsumerConfiguration>(configuration.GetSection("RabbitMQ:Queues:Consumer"));
        services.AddHostedService<UserConsumerWorker>();
    }
}