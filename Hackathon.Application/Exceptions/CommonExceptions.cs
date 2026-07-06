namespace Hackathon.Application.Exceptions;

// 400 - BadRequestException
public class BadRequestException : AppException
{
    public BadRequestException(string message)
        : base("Bad Request", 400, ErrorMessage.Common.BadRequest, message) { }

    public BadRequestException(string messageCode, string message)
        : base("Bad Request", 400, messageCode, message) { }
}

// 401 - UnauthorizedException
public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base("Unauthorized", 401, ErrorMessage.Common.Unauthorized, message) { }

    public UnauthorizedException(string messageCode, string message)
        : base("Unauthorized", 401, messageCode, message) { }
}

// 403 - ForbiddenException
public class ForbiddenException : AppException
{
    public ForbiddenException(string message)
        : base("Forbidden", 403, ErrorMessage.Common.Forbidden, message) { }

    public ForbiddenException(string messageCode, string message)
        : base("Forbidden", 403, messageCode, message) { }
}

// 404 - NotFoundException
public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base("Not Found", 404, ErrorMessage.Common.NotFound, message) { }

    public NotFoundException(string messageCode, string message)
        : base("Not Found", 404, messageCode, message) { }
}

// 409 - ConflictException
public class ConflictException : AppException
{
    public ConflictException(string message)
        : base("Conflict", 409, ErrorMessage.Common.Conflict, message) { }

    public ConflictException(string messageCode, string message)
        : base("Conflict", 409, messageCode, message) { }
}

// 429 - TooManyRequestException
public class TooManyRequestException : AppException
{
    public TooManyRequestException(string message)
        : base("Too many request", 429, ErrorMessage.Common.TooManyRequest, message) { }

    public TooManyRequestException(string messageCode, string message)
        : base("Too many request", 429, messageCode, message) { }
}

// 500 - ServerException
public class ServerException : AppException
{
    public ServerException(string message)
        : base("Internal Server Error", 500, ErrorMessage.Common.InternalServerError, message) { }

    public ServerException(string messageCode, string message)
        : base("Internal Server Error", 500, messageCode, message) { }
}

// 500 - FileUploadFailedException
public class FileUploadFailedException : AppException
{
    public FileUploadFailedException(string message)
        : base("Internal Server Error", 500, ErrorMessage.Common.FileUploadFailed, message) { }

    public FileUploadFailedException(string messageCode, string message)
        : base("Internal Server Error", 500, messageCode, message) { }
}

// 503 - ServiceUnavailableException
public class ServiceUnavailableException : AppException
{
    public ServiceUnavailableException(string message)
        : base("Service Unavailable", 503, ErrorMessage.Common.ServiceUnavailable, message) { }

    public ServiceUnavailableException(string messageCode, string message)
        : base("Service Unavailable", 503, messageCode, message) { }
}
