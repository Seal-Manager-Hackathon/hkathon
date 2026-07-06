namespace Hackathon.Service.Teams;

public static class Request
{
    public class CreateTeamRequest
    {
        public string? TeamName { get; set; }
    }

    public class InviteMemberRequest
    {
        public string? Email { get; set; }

        public string? Description { get; set; }
    }

    public class UpdateTeamRequest
    {
        public string? TeamName { get; set; }
    }

    public class RemoveMembersRequest
    {
        public List<Guid> UserIds { get; set; } = new();
    }

    public class TransferLeaderRequest
    {
        public Guid NewLeaderId { get; set; }
    }

    public class RegisterEventRequest
    {
        public Guid TeamId { get; set; }

        public Guid EventId { get; set; }

        public string? Description { get; set; }
    }

    public class GetMyRegisteredEventsRequest
    {
        public string? Status { get; set; }
    }

    public class GetMyRegistrationsByEventRequest
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "EVENT_ID_REQUIRED")]
        public Guid EventId { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "PAGE_INDEX_MUST_BE_GREATER_THAN_ZERO")]
        public int PageIndex { get; set; } = 1;

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "PAGE_SIZE_MUST_BE_GREATER_THAN_ZERO")]
        public int PageSize { get; set; } = 10;
    }

    public class RoundAppealRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public string? FileUrl { get; set; }
    }

    public class SubmissionAppealRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public string? FileUrl { get; set; }
    }
}
