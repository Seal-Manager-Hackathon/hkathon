namespace Hackathon.Application.Exceptions;

public static class ErrorMessage
{
    public static class Common
    {
        public const string UnexpectedError = "An Unexpected Error Occurred";
        public const string TooManyRequestsRetryAfter60s = "Too Many Requests Retry After 60s";
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

    public static class Events
    {
        public const string NameRequired = "Event Name Is Required";
        public const string LimitTeamNegative = "Limit Team Must Not Be Negative";
        public const string MinMemberNegative = "Min Member Must Not Be Negative";
        public const string MaxMemberNegative = "Max Member Must Not Be Negative";
        public const string MaxMemberLessThanMin = "Max Member Must Not Be Less Than Min Member";
        public const string StartTimeInPast = "Event Start Time Must Not Be In Past";
        public const string RegisterLimitTimeInPast = "Event Register Limit Time Must Not Be In Past";
        public const string EndTimeBeforeStart = "Event End Time Must Be After Start Time";
        public const string RegisterLimitTimeRange = "Event Register Limit Time Must Be Between Start And End";
    }
}
