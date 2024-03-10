namespace CoplandFileManager.Application.Interfaces;

using Domain.File.Dtos;

public interface ICreateFileUseCase
{
    Task TryCreateAsync(Stream stream, FileDto fileDto, Guid userId);
    Task<(string url, TimeSpan timeLimit)> TryCreateAsync(FileDto fileDto, Guid userId);
}