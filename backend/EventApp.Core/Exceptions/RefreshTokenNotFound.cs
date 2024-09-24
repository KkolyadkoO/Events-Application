namespace EventApp.Core.Exceptions;

public class RefreshTokenNotFound : Exception
{
    public RefreshTokenNotFound()
    {
    }

    public RefreshTokenNotFound(string message)
        : base(message)
    {
    }

    public RefreshTokenNotFound(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}