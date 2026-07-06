using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Common.Models;

public class PaginationRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "PAGE_INDEX_MUST_BE_GREATER_THAN_ZERO")]
    public int PageIndex { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PAGE_SIZE_MUST_BE_BETWEEN_1_AND_100")]
    public int PageSize { get; set; } = 10;
}