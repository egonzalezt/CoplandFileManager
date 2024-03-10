namespace CoplandFileManager.Application.UseCases;

using Domain.File.Repositories;
using Domain.File.Exceptions;
using Domain.User.Exceptions;
using Domain.User.Repository;
using Domain.StorageServiceProvider;
using Interfaces;
using System.Threading.Tasks;

public class GetSignedUrlUseCase(
        IStorageServiceProvider storageServiceProvider,
        IFileCommandRepository fileCommandRepository,
        IUserCommandRepository userCommandRepository
    ) : IGetSignedUrlUseCase
{
    public async Task<(string url, TimeSpan timeLimit)> GetSignedUrlUseCaseAsync(Guid userId, Guid fileId)
    {
        var userExists = await userCommandRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            throw new UserNotFoundException(userId.ToString());
        }
        var file = await fileCommandRepository.GetFileByIdAndUserIdAsync(fileId, userId) ?? throw new FileNotFoundException(fileId.ToString());
        var fileWithExtension = $"{file.Name}{file.Format}";
        var timeLimit = TimeSpan.FromMinutes(30);
        var url = await storageServiceProvider.GeneratePreSignedUrlAsync(fileWithExtension, userId, timeLimit);
        return (url, timeLimit);
    }
}