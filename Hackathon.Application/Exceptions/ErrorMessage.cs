namespace Hackathon.Application.Exceptions;

public static class ErrorMessage
{
    public static class Common
    {
        // 400
        public const string BadRequest = "BAD_REQUEST";
        // 401
        public const string Unauthorized = "UNAUTHORIZED";
        // 403
        public const string Forbidden = "FORBIDDEN";
        // 404
        public const string NotFound = "NOT_FOUND";
        // 409
        public const string Conflict = "CONFLICT";
        // 429
        public const string TooManyRequest = "TOO_MANY_REQUEST";
        public const string TooManyRequestRetryLater = "TOO_MANY_REQUESTS_RETRY_AFTER_60S";
        // 500
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string FileUploadFailed = "FILE_UPLOAD_FAILED";
        // 503
        public const string ServiceUnavailable = "SERVICE_UNAVAILABLE";
    }

    public static class Database
    {
        // 500
        public const string SaveChangesFailed = "SAVE_CHANGES_FAILED";
        public const string UpdateFailed = "UPDATE_FAILED";
        public const string DeleteFailed = "DELETE_FAILED";
        public const string ConcurrencyConflict = "CONCURRENCY_CONFLICT";
        public const string UniqueConstraintViolation = "UNIQUE_CONSTRAINT_VIOLATION";
        public const string ForeignKeyViolation = "FOREIGN_KEY_VIOLATION";
        public const string NotNullViolation = "NOT_NULL_VIOLATION";
    }
}
