using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Student.Team;

public class GetTeamCountRequest
{
    public bool? IsDisable { get; set; }
}

public class GetTeamEventsRequest
{
    public Guid TeamId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class CreateTeamRequest
{
    [Required(ErrorMessage = "Name Is Required")]
    [StringLength(200, ErrorMessage = "Name Must Not Exceed 200 Characters")]
    public string Name { get; set; } = null!;
}

public class UpdateTeamRequest
{
    public Guid TeamId { get; set; }

    [StringLength(200, ErrorMessage = "Name Must Not Exceed 200 Characters")]
    public string? Name { get; set; }
}

public class GetMyTeamsRequest
{
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
