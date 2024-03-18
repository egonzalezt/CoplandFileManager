namespace CoplandFileManager.Application.UseCases;

using Domain.User.Exceptions;
using Domain.File;
using Domain.File.Dtos;
using Domain.File.Repositories;
using Domain.User.Repository;
using Domain.StorageServiceProvider;
using Interfaces;
using Microsoft.Extensions.Logging;

public class CreateFileUseCase(
        IUserCommandRepository userCommandRepository,
        IFileCommandRepository fileCommandRepository,
        IFileQueryRepository fileQueryRepository,
        ILogger<CreateFileUseCase> logger,
        IStorageServiceProvider storageServiceProvider
) : ICreateFileUseCase
{

    public async Task TryCreateAsync(Stream stream, FileUploadDto fileDto, Guid userId)
    {
        logger.LogInformation("Saving File on the system");
        var userExists = await userCommandRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            throw new UserNotFoundException(userId.ToString());
        }
        bool isActive = await userCommandRepository.IsActive(userId);
        if (!isActive)
        {
            throw new UserNotActiveException(userId.ToString());
        }
        var objectRoute = File.GenerateObjectRoute(fileDto.NameWithExtension, userId);
        var fileExists = await fileCommandRepository.FileExistsByObjectRouteAsync(userId, objectRoute);
        if (!fileExists)
        {
            var newFile = File.Build(fileDto.category, fileDto.Name, fileDto.NameWithExtension, userId);
            await fileQueryRepository.CreateAsync(newFile, userId);
            logger.LogInformation("File with Id {id} will be saved on the system once the process is completed", newFile.Id);
        }
        await storageServiceProvider.UploadFileAsync(stream, objectRoute, fileDto.MimeType);
    }

    public async Task<(string url, TimeSpan timeLimit)> TryCreateAsync(FileUploadDto fileDto, Guid userId)
    {
        logger.LogInformation("Saving File on the system");
        var userExists = await userCommandRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            throw new UserNotFoundException(userId.ToString());
        }
        var objectRoute = File.GenerateObjectRoute(fileDto.NameWithExtension, userId);
        var fileExists = await fileCommandRepository.FileExistsByObjectRouteAsync(userId, objectRoute);
        if (!fileExists)
        {
            var newFile = File.Build(fileDto.category, fileDto.Name, fileDto.NameWithExtension, userId);
            await fileQueryRepository.CreateAsync(newFile, userId);
            logger.LogInformation("File with Id {id} will be saved on the system once the process is completed", newFile.Id);
        }

        var timeLimit = TimeSpan.FromMinutes(15);
        var url = await storageServiceProvider.GeneratePreSignedUrlForUploadAsync(fileDto.NameWithExtension, userId, timeLimit);
        return (url, timeLimit);
    }
}
