using System;
using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Tracks;

public static class Request
{
    public class CreateTrackRequest
    {
        [Required(ErrorMessage = "TRACK_TITLE_REQUIRED")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? MaxTeam { get; set; }
    }

    public class UpdateTrackRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? MaxTeam { get; set; }
    }

    public class UpdateTrackVisibilityRequest
    {
        public bool IsVisible { get; set; }
    }

    public class AssignTrackToTeamRequest
    {
        public Guid TrackId { get; set; }
    }

    public class AssignTopicToTeamRequest
    {
        public Guid TopicId { get; set; }
    }
}
