using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Script;

public class BulkCreateUsersRequest
{
    [Required(ErrorMessage = "Count Is Required")]
    [Range(1, 1000, ErrorMessage = "Count Must Be Between 1 And 1000")]
    public int Count { get; set; } = 1;

    [Required(ErrorMessage = "Role Is Required")]
    public string Role { get; set; } = null!;

    [Required(ErrorMessage = "EmailPrefix Is Required")]
    public string EmailPrefix { get; set; } = null!;

    public string EmailDomain { get; set; } = "@gmail.com";

    [Required(ErrorMessage = "FirstName Is Required")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "LastName Is Required")]
    public string LastName { get; set; } = null!;

    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class BulkCreateTeamRequest
{
    [Required(ErrorMessage = "LeaderEmail Is Required")]
    [EmailAddress(ErrorMessage = "Invalid Email Format")]
    public string LeaderEmail { get; set; } = null!;

    [Required(ErrorMessage = "TeamName Is Required")]
    public string TeamName { get; set; } = null!;

    public List<string> MemberEmails { get; set; } = new();
}
