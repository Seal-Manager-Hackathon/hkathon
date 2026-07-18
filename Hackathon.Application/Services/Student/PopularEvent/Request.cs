using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Student.PopularEvent;

public class GetPopularEventsRequest
{
    [Required]
    public int PageIndex { get; set; } = 1;

    [Required]
    public int PageSize { get; set; } = 10;
}
