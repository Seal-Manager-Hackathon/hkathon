namespace Hackathon.Service.Exceptions;

public class MissingAccessTokenException : AppException
{
    public MissingAccessTokenException()
        : base("Unauthorized", 401, "MISSING_ACCESS_TOKEN", 
            "ACCESS_TOKEN_IS_MISSING") { }
}

public class ExpiredAccessTokenException : AppException
{
    public ExpiredAccessTokenException()
        : base("Unauthorized", 401, "EXPIRED_ACCESS_TOKEN", 
            "Access token has expired.") { }
}

public class ExpiredRefreshTokenException : AppException
{
    public ExpiredRefreshTokenException()
        : base("Unauthorized",401,  "EXPIRED_REFRESH_TOKEN", 
            "REFRESH_TOKEN_HAS_EXPIRED") { }
}