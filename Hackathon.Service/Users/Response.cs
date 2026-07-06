using Hackathon.Repository.Enum;

namespace Hackathon.Service.Users;

public static class Reponse
{
    public class UserProfileDetailResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Address { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string College { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public string? LinkUrl { get; set; }
        public UserStatusEnum? Status { get; set; }
        public string? BanReason { get; set; }
        public RoleEnum Role { get; set; }
    }

    public class UserDetailResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Address { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string College { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public string? LinkUrl { get; set; }
        public RoleEnum Role { get; set; }
        public UserStatusEnum? Status { get; set; }
        public bool? IsVerified { get; set; }
    }

    public class StudentSearchResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string College { get; set; } = string.Empty;
        public UserStatusEnum? Status { get; set; }
    }

    public class MyAssignmentResponse
    {
        public Guid AssignEventId { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public EventRoleEnum? Role { get; set; }
        public List<AssignmentTrackResponse> Tracks { get; set; } = new();
    }

    public class AssignmentTrackResponse
    {
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
    }

    public class MyReportListItemResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? TypeReport { get; set; }
        public ReportStatusEnum? Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class MyReportDetailResponse
    {
        public Guid Id { get; set; }
        public Guid? AssignEventId { get; set; }
        public Guid? SubmissionId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public string? FileUrl { get; set; }
        public string? TypeReport { get; set; }
        public ReportStatusEnum? Status { get; set; }
        public string? Reason { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
