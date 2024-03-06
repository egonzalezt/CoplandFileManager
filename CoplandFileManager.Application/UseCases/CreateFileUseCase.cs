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

    public async Task TryCreateAsync(Stream stream, FileDto fileDto)
    {
        logger.LogInformation("Saving File on the system");
        Guid? userId = await userCommandRepository.GetIdByIdentityProviderId(fileDto.IdentityProviderUserId) ?? throw new UserNotFoundException(fileDto.IdentityProviderUserId);
        bool isActive = await userCommandRepository.IsActive((Guid)userId);
        if (!isActive)
        {
            throw new UserNotActiveException(fileDto.IdentityProviderUserId);
        }
        (var objectId, var objectRoute) = await storageServiceProvider.UploadFileAsync(stream, userId!.Value.ToString(), fileDto.Name, fileDto.MimeType);
        fileDto.ObjectId = objectId;
        fileDto.ObjectRoute = objectRoute;
        var newFile = File.Build(userId.Value, fileDto.Name, fileDto.Format, fileDto.ObjectId, fileDto.ObjectRoute);
        await fileQueryRepository.CreateAsync(newFile);
        logger.LogInformation("File with Id {id} successfully saved on the system", newFile.Id);
    }
}
