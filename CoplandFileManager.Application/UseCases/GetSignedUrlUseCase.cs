namespace CoplandFileManager.Application.UseCases;

using Domain.StorageServiceProvider;
using Interfaces;
using System.Threading.Tasks;

public class GetSignedUrlUseCase(
        IStorageServiceProvider storageServiceProvider
    ) : IGetSignedUrlUseCase
{
    public async Task<string> GetSignedUrlUseCaseAsync(string identityProviderId, string objectId)
    {
        return await storageServiceProvider.GeneratePreSignedUrlAsync(objectId, TimeSpan.FromHours(32));
    }
}