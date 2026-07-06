using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.Admin;

public class GetUsersQuery
{
    public string? KeySearch { get; set; }
    public string? MailSearch { get; set; }
    public Guid? IdSearch { get; set; }
    public RoleEnum? Role { get; set; }
    public string? StudentIdSearch { get; set; }
    public bool? IsDisable { get; set; }
    public bool? IsVerified { get; set; }
    public PaginationRequest Pagination { get; set; } = new();
}

public class GetAdminRoundsRequest : PaginationRequest
{
    public bool? IsDisable { get; set; }
}

public class ChangeUserRoleRequest
{
    public RoleEnum Role { get; set; }
}

public class SendSystemNotificationRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class CreateRoundRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public DateTimeOffset? StartSubmission { get; set; }
    public DateTimeOffset? EndSubmission { get; set; }
    public int? LimitTeam { get; set; }
}

public class UpdateRoundRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? RoundNo { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public DateTimeOffset? StartSubmission { get; set; }
    public DateTimeOffset? EndSubmission { get; set; }
    public int? LimitTeam { get; set; }
}
