namespace CoplandFileManager.Application.UseCases;

using Domain.User.Repository;
using Domain.User;
using Domain.User.Dtos;
using Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class CreateUserUseCase(ILogger<CreateUserUseCase> logger, IUserCommandRepository userCommandRepository, IUserQueryRepository userQueryRepository) : ICreateUserUseCase
{
    public async Task ExecuteAsync(UserOwnedDto userDto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user with Id: {id}", userDto.Id);
        var userExists = await userCommandRepository.ExistsByIdAsync(userDto.Id);
        if(!userExists)
        {
            var user = User.CreateUser(userDto.Id, userDto.Email);
            await userQueryRepository.CreateAsync(user, cancellationToken);
            logger.LogInformation("Created {id}", userDto.Id);
        }
    }
}
