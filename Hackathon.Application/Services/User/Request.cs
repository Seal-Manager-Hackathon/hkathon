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
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserDetailRequest
{
    public Guid UserId { get; set; }
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
