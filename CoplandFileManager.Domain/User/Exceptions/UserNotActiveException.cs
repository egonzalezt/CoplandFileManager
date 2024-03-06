namespace CoplandFileManager.Domain.User.Exceptions;

using SharedKernel.Exceptions;

public class UserNotActiveException : BusinessException
{
    public UserNotActiveException() : base("User not active on the system")
    {
    }

    public UserNotActiveException(string id) : base($"User with identification: {id} is not active on the system")
    {
    }

    public UserNotActiveException(string message, Exception innerException) : base(message, innerException)
    {
    }
}