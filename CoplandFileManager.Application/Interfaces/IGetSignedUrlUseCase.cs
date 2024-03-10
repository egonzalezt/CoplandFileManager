namespace CoplandFileManager.Application.Interfaces;

public interface IGetSignedUrlUseCase
{
    Task<(string url, TimeSpan timeLimit)> GetSignedUrlUseCaseAsync(Guid userId, Guid fileId);
}
