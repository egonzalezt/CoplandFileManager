namespace CoplandFileManager.Application.Interfaces;

using CoplandFileManager.Domain.File.Dtos;

public interface ICreateFileUseCase
{
    Task TryCreateAsync(Stream stream, FileDto fileDto);
}