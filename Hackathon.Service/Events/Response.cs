namespace Hackathon.Service.Events;

public static class Response
{
    public class EventResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? RegisterLimitTime { get; set; }
        public int? LimitTeam { get; set; }
        public int? MinMember { get; set; }
        public int? MaxMember { get; set; }
        public Hackathon.Repository.Enum.EventStatusEnum? Status { get; set; }
        public int? NumberRound { get; set; }
        public Hackathon.Repository.Enum.SeasonEnum? Season { get; set; }
        public int? Year { get; set; }
        public bool IsDisable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateEventResponse
    {
        public Guid Id { get; set; }
    }

    public class AssignStaffToEventResponse
    {
        public Guid Id { get; set; }
    }

    public class CreateAwardResponse
    {
        public Guid Id { get; set; }
    }

    public class AssignEventToTrackResponse
    {
        public Guid AssignTrackId { get; set; }
    }

    public class EventParticipantResponse : EventResponse
    {
        public int TeamCount { get; set; }
        public int ParticipantCount { get; set; }
    }

    public class StudentEventResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public Hackathon.Repository.Enum.EventStatusEnum? Status { get; set; }
        public Hackathon.Repository.Enum.SeasonEnum? Season { get; set; }
        public int? Year { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class AdminEventResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public Hackathon.Repository.Enum.EventStatusEnum? Status { get; set; }
        public Hackathon.Repository.Enum.SeasonEnum? Season { get; set; }
        public int? Year { get; set; }
        public bool IsDisable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class EventAssignmentResponse
    {
        public Guid AssignEventId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid? EventRoleId { get; set; }
        public Hackathon.Repository.Enum.EventRoleEnum? EventRoleName { get; set; }
        public List<AssignedTrackResponse> AssignedTracks { get; set; } = new();
    }

    public class AssignedTrackResponse
    {
        public Guid AssignTrackId { get; set; }
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = null!;
    }

    public class SetupStatusResponse
    {
        public bool IsReadyToPublish { get; set; }
        public SetupChecks Checks { get; set; } = new();
        public string? Message { get; set; }
    }

    public class SetupChecks
    {
        public bool HasRounds { get; set; }
        public bool HasCriteria { get; set; }
        public bool HasTracks { get; set; }
        public bool HasTopics { get; set; }
        public bool HasAwards { get; set; }
        public bool HasAssignedStaff { get; set; }
    }

    public class AwardResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? LevelAward { get; set; }
        public int? NumberOfAward { get; set; }
        public decimal? Prize { get; set; }
    }

    public class LeaderboardResponse
    {
        public int Rank { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public decimal? TotalScore { get; set; }
        public int? LevelAward { get; set; }
        public List<RoundScoreResponse> RoundScores { get; set; } = new();
    }

    public class RoundScoreResponse
    {
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = null!;
        public int? RoundNo { get; set; }
        public decimal? AverageScore { get; set; }
    }

    public class EventSummaryResponse
    {
        public int TotalApprovedTeams { get; set; }
        public int TotalTracks { get; set; }
        public int TotalRounds { get; set; }
        public int TotalAwards { get; set; }
    }

    public class TeamScoreResponse
    {
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = null!;
        public int? RoundNo { get; set; }
        public decimal? AverageTotalScore { get; set; }
        public List<CriteriaScoreResponse> CriteriaScores { get; set; } = new();
    }

    public class AvailableStaffResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }

    public class CriteriaScoreResponse
    {
        public Guid CriteriaItemId { get; set; }
        public string CriteriaItemName { get; set; } = null!;
        public decimal? AverageCriteriaScore { get; set; }
        public decimal MaxScore { get; set; }
    }
}
