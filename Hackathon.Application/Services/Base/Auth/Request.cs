using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Base.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email Is Required")]
    [EmailAddress(ErrorMessage = "Invalid Email Format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password Is Required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password Must Be Between 6 And 100 Characters")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "First Name Is Required")]
    [StringLength(50, ErrorMessage = "First Name Must Not Exceed 50 Characters")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last Name Is Required")]
    [StringLength(50, ErrorMessage = "Last Name Must Not Exceed 50 Characters")]
    public string LastName { get; set; } = null!;
}

public class VerifyEmailRequest
{
    [Required(ErrorMessage = "Token Is Required")]
    public string Token { get; set; } = null!;
}

public class LoginRequest
{
    [Required(ErrorMessage = "Email Is Required")]
    [EmailAddress(ErrorMessage = "Invalid Email Format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password Is Required")]
    public string Password { get; set; } = null!;
}

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "Email Is Required")]
    [EmailAddress(ErrorMessage = "Invalid Email Format")]
    public string Email { get; set; } = null!;
}

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "Token Is Required")]
    public string Token { get; set; } = null!;

    [Required(ErrorMessage = "New Password Is Required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password Must Be Between 6 And 100 Characters")]
    public string NewPassword { get; set; } = null!;
}

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Current Password Is Required")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "New Password Is Required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password Must Be Between 6 And 100 Characters")]
    public string NewPassword { get; set; } = null!;
}

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh Token Is Required")]
    public string RefreshToken { get; set; } = null!;
}
