namespace EventApp.Core.Exceptions;

public class InvalidRefreshToken : Exception
{
    public InvalidRefreshToken()
    {
    }

    public InvalidRefreshToken(string message)
        : base(message)
    {
    }

    public InvalidRefreshToken(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}