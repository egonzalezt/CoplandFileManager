namespace CoplandFileManager.Domain.User.Exceptions;

using SharedKernel.Exceptions;

public class UserNotFoundException : BusinessException
{
    public UserNotFoundException() : base("User not found on the system")
    {
    }

    public UserNotFoundException(string id) : base($"User with identification: {id} not found on the system")
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}