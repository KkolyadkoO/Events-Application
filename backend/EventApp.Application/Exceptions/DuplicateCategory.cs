namespace EventApp.Application.Exceptions;

public class DuplicateCategory : Exception
{
    public DuplicateCategory()
    {
    }

    public DuplicateCategory(string message)
        : base(message)
    {
    }

    public DuplicateCategory(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}