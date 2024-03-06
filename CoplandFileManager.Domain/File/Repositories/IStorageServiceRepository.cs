namespace CoplandFileManager.Domain.File.Repositories;

public interface IStorageServiceRepository
{
    Task<string> GeneratePreSignedURlAsync(string objectId);
}
