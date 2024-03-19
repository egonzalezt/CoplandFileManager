namespace CoplandFileManager.Workers.MessageBroker;

using Domain.User.Dtos;
using Domain.User;
using Workers.Exceptions;
using Workers.Extensions;
using Workers.MessageBroker.Options;
using Application.Interfaces;
using Frieren_Guard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Infrastructure.EntityFrameworkCore.DbContext;
using CoplandFileManager.Domain.File.Dtos;
using System;

internal class UserTransferConsumerWorker(
    ILogger<UserTransferConsumerWorker> logger,
    IServiceProvider serviceProvider,
    ConnectionFactory rabbitConnection,
    IHealthCheckNotifier healthCheckNotifier,
    SystemStatusMonitor statusMonitor,
    IOptions<ConsumerConfiguration> queues,
    IOptions<PublisherConfiguration> publisherQueues
    ) : BaseRabbitMQWorker(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.BebopUserTransferRequestQueue)
{
    private readonly PublisherConfiguration PublisherQueues = publisherQueues.Value;
    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel, CancellationToken stoppingToken)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        using var scope = serviceProvider.CreateScope();

        if (operation is UserOperations.TransferUser)
        {
            var userDto = JsonSerializer.Deserialize<UserTransferRequestDto>(message) ?? throw new InvalidBodyException();
            logger.LogInformation("Processing request for user {userId}", userDto.UserId);
            var registerUserUseCase = scope.ServiceProvider.GetRequiredService<ITransferUserUseCase>();
            (var files, var userDeactivated) = await registerUserUseCase.ExecuteAsync(userDto.UserId);
            if (userDeactivated)
            {
                await PublishUnregisterUserRequestToBebopAsync(userDto.UserId, files, channel, scope);
            }
            else
            {
                logger.LogWarning("Cannot deactivate user with Id: {id}", userDto.UserId);
            }
            channel.BasicAck(eventArgs.DeliveryTag, false);
        }
    }

    private async Task PublishUnregisterUserRequestToBebopAsync(Guid userId, IEnumerable<UserFileTransferDto>? files, 
        IModel channel, IServiceScope scope)
    {
        if (files is null)
        {
            logger.LogWarning("User with Id {id} not found on the system", userId);
            return;
        }
        var requestHeaders = new EventHeaders(UserOperations.CoplandResponse.ToString(), userId);
        var properties = channel.CreateBasicProperties();
        properties.Headers = requestHeaders.GetAttributesAsDictionary();
        string jsonResult = JsonSerializer.Serialize(files);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonResult);
        channel.BasicPublish("", PublisherQueues.BebopUserTransferReplyQueue, properties, jsonBytes);
        var database = scope.ServiceProvider.GetRequiredService<CoplandFileManagerDbContext>();
        await database.SaveChangesAsync();
    }
}
