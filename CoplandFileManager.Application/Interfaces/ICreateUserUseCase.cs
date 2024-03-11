namespace CoplandFileManager.Application.Interfaces;

using Domain.User;
using Domain.User.Dtos;

public interface ICreateUserUseCase
{
    Task ExecuteAsync(UserOwnedDto userDto, CancellationToken cancellationToken);
}
