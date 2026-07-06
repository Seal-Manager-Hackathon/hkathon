using System.ComponentModel.DataAnnotations;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Common.Models;

public class PaginationRequest
{
    [Range(1, int.MaxValue, ErrorMessage = ErrMsg.Pagination.PageIndexMustBeGreaterThanZero)]
    public int PageIndex { get; set; } = 1;

    [Range(1, 100, ErrorMessage = ErrMsg.Pagination.PageSizeMustBeBetween1And100)]
    public int PageSize { get; set; } = 10;
}
