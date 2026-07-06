using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.User;

public class GetUserCountRequest
{
    public string? Role { get; set; }
}

public class GetAllUsersRequest
{
    public string? Keyword { get; set; }
    public string? Role { get; set; }
    public bool? IsDisable { get; set; }
    public bool? IsVerified { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserDetailRequest
{
    public Guid UserId { get; set; }
}

public class UpdateUserRequest
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public Microsoft.AspNetCore.Http.IFormFile? AvatarFile { get; set; }
    public string? Bio { get; set; }
    public string? Address { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public string? StudentId { get; set; }
    public string? College { get; set; }
    public string? ImgUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string? Status { get; set; }
    public bool? IsDisable { get; set; }
}

public class CreateUserRequest
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

    [Required(ErrorMessage = "Role Is Required")]
    public string Role { get; set; } = null!;

    public string? College { get; set; }
}
