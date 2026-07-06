namespace Hackathon.Application.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string message)
        : base("Bad Request", 400, message) { }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base("Unauthorized", 401, message) { }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message)
        : base("Forbidden", 403, message) { }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base("Not Found", 404, message) { }
}

public class ConflictException : AppException
{
    public ConflictException(string message)
        : base("Conflict", 409, message) { }
}

public class TooManyRequestException : AppException
{
    public TooManyRequestException(string message)
        : base("Too Many Request", 429, message) { }
}

public class ServerException : AppException
{
    public ServerException(string message)
        : base("Internal Server Error", 500, message) { }
}

public class FileUploadFailedException : AppException
{
    public FileUploadFailedException(string message)
        : base("Internal Server Error", 500, message) { }
}

public class ServiceUnavailableException : AppException
{
    public ServiceUnavailableException(string message)
        : base("Service Unavailable", 503, message) { }
}
