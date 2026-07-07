namespace Hackathon.Application.Exceptions;

public static class ErrorMessage
{
    public static class Event
    {
        public const string EndTimeMustBeAfterStartTime = "End Time Must Be After Start Time";
        public const string StartTimeMustBeAfterNow = "Start Time Must Be After Current Time";
        public const string RegisterLimitTimeMustBeBeforeEndTime = "Register Limit Time Must Be Before End Time";
        public const string RegisterLimitTimeMustBeAfterStartTime = "Register Limit Time Must Be After Start Time";
    }

    public static class Common
    {
        public const string UnexpectedError = "An Unexpected Error Occurred";
        public const string TooManyRequestsRetryAfter60S = "Too Many Requests Retry After 60s";
        public const string InvalidRequestData = "Invalid Request Data";
        public const string InvalidJsonFormat = "Invalid Json Format";
        public const string InvalidEnumValue = "Invalid Enum Value";
        public const string InvalidDataType = "Invalid Data Type";
    }

    public static class Round
    {
        public const string EndTimeMustBeAfterStartTime = "Round End Time Must Be After Start Time";
        public const string StartSubmissionMustBeAfterStartTime = "Start Submission Must Be After Or Equal Start Time";
        public const string EndSubmissionMustBeBeforeEndTime = "End Submission Must Be Before Or Equal End Time";
        public const string LimitTeamMustBeAtLeast1 = "Limit Team Must Be At Least 1";
        public const string RoundTimeMustBeWithinEventTime = "Round Time Must Be Within Event Time Range";
        public const string StartTimeMustBeAfterRegisterLimitTime = "Round Start Time Must Be After Event Register Limit Time";
        public const string RoundStartTimeMustBeAfterPreviousRoundEndTime = "Round Start Time Must Be After Or Equal Previous Round End Time";
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

    public static class Auth
    {
        public const string InvalidOrExpiredToken = "Invalid Or Expired Token";
        public const string TokenValidationFailed = "Token Validation Failed";
        public const string RefreshTokenMissing = "Refresh Token Missing";
        public const string AccessTokenStillValid = "Access Token Still Valid";
        public const string EmailAlreadyExists = "Email Already Exists";
        public const string UserNotFound = "User Not Found";
        public const string InvalidEmailOrPassword = "Invalid Email Or Password";
        public const string UnverifiedAccountPleaseLoginToVerify = "Unverified Account Please Login To Verify";
        public const string EmailUnverifiedOtpSent = "Email Unverified Otp Sent";
        public const string UserIsBanned = "User Is Banned";
        public const string UserAlreadyLoggedOut = "User Already Logged Out";
        public const string InvalidRefreshToken = "Invalid Refresh Token";
        public const string UserAlreadyVerified = "User Already Verified";
        public const string CurrentPasswordInvalid = "Current Password Invalid";
        public const string NewPasswordMustBeDifferentFromOldPassword = "New Password Must Be Different From Old Password";
        public const string InvalidOrExpiredEmailVerificationToken = "Invalid Or Expired Email Verification Token";
    }

    public static class Mail
    {
        public const string SendFailed = "Failed To Send Email Please Try Again Later";
    }

    public static class Media
    {
        public const string FileEmpty = "File Is Empty";
        public const string InvalidImageFormat = "Invalid Image Format Only Jpg Jpeg Png Gif Webp Are Allowed";
        public const string FileTooLarge = "File Is Too Large Maximum 50Mb";
        public const string UploadFailed = "Image Upload Failed Please Try Again Later";
    }
}
