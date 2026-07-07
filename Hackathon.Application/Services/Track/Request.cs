using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Track;

public class CreateTrackRequest
{
    [Required(ErrorMessage = "EventId Is Required")]
    public Guid EventId { get; set; }

    [Required(ErrorMessage = "Title Is Required")]
    [StringLength(200, ErrorMessage = "Title Must Not Exceed 200 Characters")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "MaxTeam Must Be Greater Than Or Equal To 0")]
    public int? MaxTeam { get; set; }
}

public class UpdateTrackRequest
{
    public Guid TrackId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? MaxTeam { get; set; }

    public bool? IsDisable { get; set; }
}

public class CreateTrackResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
}

public class GetTracksByEventRequest
{
    [Required(ErrorMessage = "EventId Is Required")]
    public Guid EventId { get; set; }

    public string? Keyword { get; set; }

    public bool? IsDisable { get; set; }

    public int PageIndex { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize Must Be Between 1 And 100")]
    public int PageSize { get; set; } = 10;
}

public class GetTracksByEventResponse
{
    public List<TrackItem> Tracks { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetTrackDetailResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
    public int RegisterTeamCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class TrackItem
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
