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

    public static class Auth
    {
        public const string InvalidOrExpiredToken = "Invalid Or Expired Token";
        public const string TokenValidationFailed = "Token Validation Failed";
        public const string RefreshTokenMissing = "Refresh Token Missing";
        public const string AccessTokenStillValid = "Access Token Still Valid";
        public const string EmailAlreadyExists = "Email Already Exists";
        public const string UserNotFound = "User Not Found";
        public const string InvalidEmailOrPassword = "Invalid Email Or Password";
        public const string RegistrationSuccessful = "Registration Successful";
        public const string LogoutSuccessful = "Logout Successful";
        public const string PasswordChangedSuccessfully = "Password Changed Successfully";
        public const string EmailVerificationSent = "Email Verification Sent";
        public const string ForgotPasswordRequestAccepted = "Forgot Password Request Accepted";
        public const string PasswordResetSuccessfully = "Password Reset Successfully";
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

}
