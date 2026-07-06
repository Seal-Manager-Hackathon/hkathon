namespace Hackathon.Application.Exceptions;

public static class ErrorMessage
{
    public static class Common
    {
        public const string UnexpectedError = "An Unexpected Error Occurred";
        public const string TooManyRequestsRetryAfter60S = "Too Many Requests Retry After 60s";
    }

    public static class Database
    {
        public const string SaveChangesFailed = "Save Changes Failed";
        public const string UpdateFailed = "Update Failed";
        public const string DeleteFailed = "Delete Failed";
        public const string ConcurrencyConflict = "Concurrency Conflict";
        public const string UniqueConstraintViolation = "Unique Constraint Violation";
        public const string ForeignKeyViolation = "Foreign Key Violation";
        public const string NotNullViolation = "Not Null Violation";
    }

    public static class Pagination
    {
        public const string PageIndexMustBeGreaterThanZero = "Page Index Must Be Greater Than Zero";
        public const string PageSizeMustBeBetween1And100 = "Page Size Must Be Between 1 And 100";
    }

}
