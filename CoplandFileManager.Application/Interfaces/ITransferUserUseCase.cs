using CoplandFileManager.Domain.File.Dtos;

namespace CoplandFileManager.Application.Interfaces;

public interface ITransferUserUseCase
{
    Task<(IEnumerable<UserFileTransferDto> files, bool userDeactivated)> ExecuteAsync(Guid userId);
}