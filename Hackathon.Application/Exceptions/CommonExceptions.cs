namespace Hackathon.Application.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string message)
        : base("Bad Request", 400, ErrorCode4Xx.BadRequest, message) { }

    public BadRequestException(string messageCode, string message)
        : base("Bad Request", 400, messageCode, message) { }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base("Unauthorized", 401, ErrorCode4Xx.Unauthorized, message) { }

    public UnauthorizedException(string messageCode, string message)
        : base("Unauthorized", 401, messageCode, message) { }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message)
        : base("Forbidden", 403, ErrorCode4Xx.Forbidden, message) { }

    public ForbiddenException(string messageCode, string message)
        : base("Forbidden", 403, messageCode, message) { }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base("Not Found", 404, ErrorCode4Xx.NotFound, message) { }

    public NotFoundException(string messageCode, string message)
        : base("Not Found", 404, messageCode, message) { }
}

public class ConflictException : AppException
{
    public ConflictException(string message)
        : base("Conflict", 409, ErrorCode4Xx.Conflict, message) { }

    public ConflictException(string messageCode, string message)
        : base("Conflict", 409, messageCode, message) { }
}

public class TooManyRequestException : AppException
{
    public TooManyRequestException(string message)
        : base("Too Many Request", 429, ErrorCode4Xx.TooManyRequest, message) { }

    public TooManyRequestException(string messageCode, string message)
        : base("Too Many Request", 429, messageCode, message) { }
}

public class ServerException : AppException
{
    public ServerException(string message)
        : base("Internal Server Error", 500, ErrorCode5Xx.InternalServerError, message) { }

    public ServerException(string messageCode, string message)
        : base("Internal Server Error", 500, messageCode, message) { }
}

public class FileUploadFailedException : AppException
{
    public FileUploadFailedException(string message)
        : base("Internal Server Error", 500, ErrorCode5Xx.FileUploadFailed, message) { }

    public FileUploadFailedException(string messageCode, string message)
        : base("Internal Server Error", 500, messageCode, message) { }
}

public class ServiceUnavailableException : AppException
{
    public ServiceUnavailableException(string message)
        : base("Service Unavailable", 503, ErrorCode5Xx.ServiceUnavailable, message) { }

    public ServiceUnavailableException(string messageCode, string message)
        : base("Service Unavailable", 503, messageCode, message) { }
}
