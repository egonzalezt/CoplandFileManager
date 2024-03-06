namespace CoplandFileManager.Application.Interfaces;

public interface IGetSignedUrlUseCase
{
    Task<string> GetSignedUrlUseCaseAsync(string identityProviderId, string objectId);
}
