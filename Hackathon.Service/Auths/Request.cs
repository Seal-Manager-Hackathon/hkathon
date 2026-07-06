using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Auths;

public class Request
{
    public class RegisterRequest
    {
        // [Required(ErrorMessage = "FIRST_NAME_REQUIRED")]
        public required string FirstName { get; set; }

        // [Required(ErrorMessage = "LAST_NAME_REQUIRED")]
        public required string LastName { get; set; }

        // [Required(ErrorMessage = "EMAIL_REQUIRED")]
        // [EmailAddress(ErrorMessage = "INVALID_EMAIL_FORMAT")]
        public required string Email { get; set; }

        // [Required(ErrorMessage = "PASSWORD_REQUIRED")]
        // [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$", ErrorMessage = "INVALID_PASSWORD_FORMAT")]
        public required string Password { get; set; }

        // [Required(ErrorMessage = "CONFIRM_PASSWORD_REQUIRED")]
        // [Compare(nameof(Password), ErrorMessage = "PASSWORD_CONFIRMATION_NOT_MATCH")]
        public required string ConfirmPassword { get; set; }
    }
    public class VerifyEmailRequest
    {
        // [Required(ErrorMessage = "TOKEN_REQUIRED")]
        public required string Token { get; set; }

    }

    public class LoginRequest
    {
        // [Required(ErrorMessage = "Email không được để trống")]
        // [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public required string Email { get; set; }

        // [Required(ErrorMessage = "Mật khẩu không được để trống")]

        public required string Password { get; set; }
    }

    public class ChangePasswordRequest
    {
        // [Required(ErrorMessage = "CURRENT_PASSWORD_REQUIRED")]
        public required string CurrentPassword { get; set; }

        // [Required(ErrorMessage = "NEW_PASSWORD_REQUIRED")]
        // [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$", ErrorMessage = "INVALID_PASSWORD_FORMAT")]
        public required string NewPassword { get; set; }

        // [Required(ErrorMessage = "CONFIRM_PASSWORD_REQUIRED")]
        // [Compare(nameof(NewPassword), ErrorMessage = "PASSWORD_CONFIRMATION_NOT_MATCH")]
        public required string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordRequest
    {
        // [Required(ErrorMessage = "EMAIL_REQUIRED")]
        // [EmailAddress(ErrorMessage = "INVALID_EMAIL_FORMAT")]
        public required string Email { get; set; }
    }

    public class ResendEmailVerificationRequest
    {
        // [Required(ErrorMessage = "EMAIL_REQUIRED")]
        // [EmailAddress(ErrorMessage = "INVALID_EMAIL_FORMAT")]
        public required string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        // [Required(ErrorMessage = "Token không được để trống")]
        public required string Token { get; set; }

        // [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        public required string NewPassword { get; set; }

        // [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        // [Compare(nameof(NewPassword), ErrorMessage = "PASSWORD_CONFIRMATION_NOT_MATCH")]
        public required string ConfirmPassword { get; set; }
    }
}