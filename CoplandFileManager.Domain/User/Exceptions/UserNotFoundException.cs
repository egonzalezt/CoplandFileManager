namespace CoplandFileManager.Domain.User.Exceptions;

using SharedKernel.Exceptions;

public class UserNotFoundException : BusinessException
{
    public UserNotFoundException() : base("Lain not found the user on the system")
    {
    }

    public UserNotFoundException(string id) : base($"Lain does not know about you with identification: {id}")
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}