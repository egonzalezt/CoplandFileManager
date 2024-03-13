namespace CoplandFileManager.Application.Interfaces;

using Domain.File.Dtos;

public interface ICreateFileUseCase
{
    Task TryCreateAsync(Stream stream, FileUploadDto fileDto, Guid userId);
    Task<(string url, TimeSpan timeLimit)> TryCreateAsync(FileUploadDto fileDto, Guid userId);
}