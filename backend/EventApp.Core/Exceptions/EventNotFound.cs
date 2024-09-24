namespace EventApp.Core.Exceptions;

public class EventNotFound : Exception
{
    public EventNotFound()
    {
    }

    public EventNotFound(string message)
        : base(message)
    {
    }

    public EventNotFound(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}