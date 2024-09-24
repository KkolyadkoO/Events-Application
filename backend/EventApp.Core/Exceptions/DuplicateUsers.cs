namespace EventApp.Core.Exceptions;

public class DuplicateUsers : Exception
{
    public DuplicateUsers()
    {
    }

    public DuplicateUsers(string message)
        : base(message)
    {
    }

    public DuplicateUsers(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}