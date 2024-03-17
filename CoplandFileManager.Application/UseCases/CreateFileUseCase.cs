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
        await storageServiceProvider.UploadFileAsync(stream, objectRoute, fileDto.MimeType);
        var newFile = File.Build(fileDto.category, fileDto.Name, fileDto.NameWithExtension, userId);
        await fileQueryRepository.CreateAsync(newFile, userId);
        logger.LogInformation("File with Id {id} successfully saved on the system", newFile.Id);
    }

    public async Task<(string url, TimeSpan timeLimit)> TryCreateAsync(FileUploadDto fileDto, Guid userId)
    {
        logger.LogInformation("Saving File on the system");
        var userExists = await userCommandRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            throw new UserNotFoundException(userId.ToString());
        }
        var newFile = File.Build(fileDto.category, fileDto.Name, fileDto.NameWithExtension, userId);
        await fileQueryRepository.CreateAsync(newFile, userId);
        var timeLimit = TimeSpan.FromMinutes(15);
        var url = await storageServiceProvider.GeneratePreSignedUrlForUploadAsync(fileDto.NameWithExtension, userId, timeLimit);
        logger.LogInformation("File with Id {id} successfully saved on the system", newFile.Id);
        return (url, timeLimit);
    }
}
