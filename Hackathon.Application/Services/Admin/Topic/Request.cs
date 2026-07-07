using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Admin.Topic;

public class CreateTopicRequest
{
    [Required(ErrorMessage = "TrackId Is Required")]
    public Guid TrackId { get; set; }

    [Required(ErrorMessage = "Title Is Required")]
    [StringLength(200, ErrorMessage = "Title Must Not Exceed 200 Characters")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}

public class CreateTopicResponse
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
}

public class UpdateTopicRequest
{
    public Guid TopicId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsDisable { get; set; }
}

public class GetTopicsByTrackRequest
{
    [Required(ErrorMessage = "TrackId Is Required")]
    public Guid TrackId { get; set; }

    public string? Keyword { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    [Range(1, 100, ErrorMessage = "PageSize Must Be Between 1 And 100")]
    public int PageSize { get; set; } = 10;
}

public class GetTopicsByTrackResponse
{
    public List<TopicItem> Topics { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetTopicDetailResponse
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public string TrackTitle { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class TopicItem
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public string TrackTitle { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
