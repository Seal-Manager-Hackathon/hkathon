using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Staff.Award;

public class GetAwardsRequest
{
    public string? Keyword { get; set; }

    public int PageIndex { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize Must Be Between 1 And 100")]
    public int PageSize { get; set; } = 10;
}