namespace Hackathon.Application.Exceptions;

public abstract class AppException : Exception
{
    public string Title { get; }
    public int StatusCode { get; }

    protected AppException(string title, int statusCode, string message)
        : base(message)
    {
        Title = title;
        StatusCode = statusCode;
    }
}
