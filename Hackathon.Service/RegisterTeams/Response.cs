using Hackathon.Repository.Enum;

namespace Hackathon.Service.RegisterTeams;

public static class Response
{
    public class RegisterTeamResponse
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid EventId { get; set; }
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public string? Description { get; set; }
        public string? RejectionReason { get; set; }
        public RegisterTeamStatusEnum? Status { get; set; }
        public bool IsBanned { get; set; }
        public bool IsDisable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class RegisterTeamDetailResponse
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public string? Description { get; set; }
        public string? RejectionReason { get; set; }
        public RegisterTeamStatusEnum Status { get; set; }
        public bool IsBanned { get; set; }
        public bool IsDisable { get; set; }
        public bool IsEliminated { get; set; }
        public Guid? CurrentRoundId { get; set; }
        public string? CurrentRoundName { get; set; }
        public int? CurrentRoundNo { get; set; }
        public List<RegisterTeamMemberResponse> Members { get; set; } = new();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class RegisterTeamMemberResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public bool IsLeader { get; set; }
    }

    public class RegisterTeamActionResponse
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public RegisterTeamStatusEnum Status { get; set; }
        public string? RejectionReason { get; set; }
        public bool IsBanned { get; set; }
    }

    public class RegisterTeamRejectionReasonResponse
    {
        public Guid RegisterId { get; set; }
        public Guid TeamId { get; set; }
        public Guid EventId { get; set; }
        public RegisterTeamStatusEnum Status { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class RegisterTeamDetailForStudentResponse
    {
        public Guid RegisterId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public RegisterTeamStatusEnum? Status { get; set; }
        public string? Description { get; set; }
        public string? RejectionReason { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class RegisteredEventItemResponse
    {
        public Guid RegisterId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public RegisterTeamStatusEnum? Status { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class RegisterTeamTrackResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public RegisterTeamStatusEnum Status { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public Guid? CurrentRoundId { get; set; }
        public string? CurrentRoundName { get; set; }
        public int? CurrentRoundNo { get; set; }
        public bool IsEliminated { get; set; }
    }

    public class RegisterTeamApprovedResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public Guid? CurrentRoundId { get; set; }
        public string? CurrentRoundName { get; set; }
        public int? CurrentRoundNo { get; set; }
        public bool IsEliminated { get; set; }
    }

    public class RegisterTeamByRoundResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public RegisterTeamStatusEnum Status { get; set; }
        public bool IsBanned { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class TeamRoundSubmissionResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public List<SubmissionDetailDto> Submissions { get; set; } = new();
    }

    public class SubmissionDetailDto
    {
        public Guid SubmissionId { get; set; }
        public Guid RoundId { get; set; }
        public int? RoundNo { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public Repository.Enum.SubmissionStatusEnum? Status { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public bool IsLatest { get; set; }
        public string GradingStatus { get; set; } = "NotGraded";
        public SubmissionScoreDto? Score { get; set; }
    }

    public class SubmissionScoreDto
    {
        public Guid ScoreId { get; set; }
        public decimal? TotalScore { get; set; }
        public bool IsRetake { get; set; }
        public bool IsMock { get; set; }
        public List<ScoreItemDto> ScoreItems { get; set; } = new();
    }

    public class ScoreItemDto
    {
        public Guid ScoreItemId { get; set; }
        public Guid CriteriaItemId { get; set; }
        public string CriteriaItemName { get; set; } = string.Empty;
        public decimal? Score { get; set; }
        public decimal MaxScore { get; set; }
        public string? Comment { get; set; }
    }

    public class RegisterTeamAssignmentStatusResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public RegisterTeamStatusEnum? Status { get; set; }
        public bool IsApproved { get; set; }
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public string? TrackDescription { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public string? TopicDescription { get; set; }
    }
}
