namespace CoplandFileManager.Application.Interfaces;

public interface IGetSignedUrlUseCase
{
    Task<(string url, TimeSpan timeLimit)> GetSignedUrlUseCaseAsync(Guid userId, Guid fileId);
    Task<(string url, TimeSpan timeLimit)> GetSignedUrlForTransferAsync(Guid userId, string objectRoute, TimeSpan timeLimit);
}
