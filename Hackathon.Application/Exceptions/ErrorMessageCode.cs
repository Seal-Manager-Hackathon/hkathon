namespace Hackathon.Application.Exceptions;

public static class ErrorCode4Xx
{
    /// <summary>400 Bad Request</summary>
    public const string BadRequest = "Bad Request";
    /// <summary>401 Unauthorized</summary>
    public const string Unauthorized = "Unauthorized";
    /// <summary>403 Forbidden</summary>
    public const string Forbidden = "Forbidden";
    /// <summary>404 Not Found</summary>
    public const string NotFound = "Not Found";
    /// <summary>409 Conflict</summary>
    public const string Conflict = "Conflict";
    /// <summary>429 Too Many Requests</summary>
    public const string TooManyRequest = "Too Many Request";
}

public static class ErrorCode5Xx
{
    /// <summary>500 Internal Server Error</summary>
    public const string InternalServerError = "Internal Server Error";
    /// <summary>500 File Upload Failed</summary>
    public const string FileUploadFailed = "File Upload Failed";
    /// <summary>503 Service Unavailable</summary>
    public const string ServiceUnavailable = "Service Unavailable";
}
