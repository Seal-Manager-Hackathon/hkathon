using Hackathon.Application.Exceptions;

namespace Hackathon.Application.Common.Helpers;

public static class PaginationHelper
{
    public static void Validate(int pageIndex, int pageSize)
    {
        if (pageIndex < 1)
            throw new BadRequestException(ErrorMessage.Pagination.PageIndexMustBeGreaterThanZero);
        if (pageSize < 1 || pageSize > 100)
            throw new BadRequestException(ErrorMessage.Pagination.PageSizeMustBeBetween1And100);
    }
}
