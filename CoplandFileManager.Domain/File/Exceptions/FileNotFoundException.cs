namespace CoplandFileManager.Domain.File.Exceptions;

using SharedKernel.Exceptions;

public class FileNotFoundException : BusinessException
{
    public FileNotFoundException() : base("Lain not found the user for the selected user")
    {
    }

    public FileNotFoundException(string id) : base($"Lain not found the file with {id} for the current user")
    {
    }

    public FileNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
