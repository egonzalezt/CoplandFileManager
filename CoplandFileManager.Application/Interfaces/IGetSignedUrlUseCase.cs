namespace CoplandFileManager.Application.Interfaces;

public interface IGetSignedUrlUseCase
{
    Task<(string url, TimeSpan timeLimit)> GetSignedUrlUseCaseAsync(string identityProviderUserId, Guid fileId);
}
