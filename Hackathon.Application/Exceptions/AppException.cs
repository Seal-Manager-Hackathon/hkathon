namespace Hackathon.Application.Exceptions;

public abstract class AppException: Exception
{
    public string Title { get; set; } 
    public int StatusCode { get; set; }
    public string MessageCode { get; set; }
    
    protected AppException(
        string title, int statusCode, string messageCode,  string message): base(message)
    {
        Title = title;
        StatusCode = statusCode;
        MessageCode = messageCode;
    }

}
