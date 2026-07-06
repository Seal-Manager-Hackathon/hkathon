using System.ComponentModel.DataAnnotations;
using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.Events;

public static class Request
{
    public class GetEventsRequest : PaginationRequest
    {
        public string? Keyword { get; set; }
        public int? Year { get; set; }
        public string? Status { get; set; }
    }

    public class GetEventsForAdminRequest : PaginationRequest
    {
        public string? Keyword { get; set; }
        public int? Year { get; set; }
        public string? Status { get; set; }
        public bool? IsDisable { get; set; }
    }

    public class GetJoinedEventsRequest : PaginationRequest
    {
        public string? Keyword { get; set; }
        public int? Year { get; set; }
        public string? Status { get; set; }
    }

    public class CreateEventRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? RegisterLimitTime { get; set; }
        public int? LimitTeam { get; set; }
        public int? MinMember { get; set; }
        public int? MaxMember { get; set; }
        public Hackathon.Repository.Enum.SeasonEnum? Season { get; set; }
    }

    public class UpdateEventRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? RegisterLimitTime { get; set; }
        public int? LimitTeam { get; set; }
        public int? MinMember { get; set; }
        public int? MaxMember { get; set; }
        public EventStatusEnum? Status { get; set; }
        public Hackathon.Repository.Enum.SeasonEnum? Season { get; set; }
    }

    public class AssignStaffToEventRequest
    {
        public Guid UserId { get; set; }
    }

    public class CreateAwardRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? LevelAward { get; set; }
        public int? NumberOfAward { get; set; }
        public decimal? Prize { get; set; }
    }

    public class AssignEventToTrackRequest
    {
        public Guid TrackId { get; set; }
    }

    public class UpdateAwardRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? LevelAward { get; set; }
        public int? NumberOfAward { get; set; }
        public decimal? Prize { get; set; }
    }

    public class UpdateLecturerRoleRequest
    {
        public EventRoleEnum? EventRole { get; set; }
    }
}
