using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Student.Award;

public class GetAwardsRequest
{
    [Required(ErrorMessage = "EventId Is Required")]
    public Guid EventId { get; set; }

    public string? Keyword { get; set; }

    public int PageIndex { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize Must Be Between 1 And 100")]
    public int PageSize { get; set; } = 10;
}
