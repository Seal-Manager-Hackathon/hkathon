using Hackathon.Repository.Enum;

namespace Hackathon.Service.Admin;

public class AdminUserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string? StudentId { get; set; }
    public string? College { get; set; }
    public RoleEnum Role { get; set; }
    public UserStatusEnum? Status { get; set; }
    public bool? IsVerified { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class AdminRoundResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? RoundNo { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public DateTimeOffset? StartSubmission { get; set; }
    public DateTimeOffset? EndSubmission { get; set; }
    public int? LimitTeam { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class CreateRoundResponse
{
    public Guid RoundId { get; set; }
}

public class SendSystemNotificationResponse
{
    public List<Guid> NotificationIds { get; set; } = new();
    public int TotalSent { get; set; }
}

public class AdminRoundTeamSubmissionResponse
{
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public List<AdminSubmissionHistoryResponse> Submissions { get; set; } = new();
    public bool HasLatestSubmission { get; set; }
    public decimal? AverageScore { get; set; }
    public string? GradingStatus { get; set; }
    public List<Rounds.Response.AssignedJudgeResponse> AssignedJudges { get; set; } = new();
}

public class AdminSubmissionHistoryResponse
{
    public Guid SubmissionId { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public Hackathon.Repository.Enum.SubmissionStatusEnum? Status { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public bool IsLatest { get; set; }
    public decimal? AverageScore { get; set; }
    public string? GradingStatus { get; set; }
    public List<Rounds.Response.AssignedJudgeResponse> AssignedJudges { get; set; } = new();
}
