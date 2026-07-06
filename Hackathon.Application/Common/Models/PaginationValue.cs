namespace Hackathon.Application.Common.Models;

public class PaginationValue<T>
{
    public List<T> Items { get; set; } = new();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasNextPage => PageIndex * PageSize < TotalCount;
    public bool HasPreviousPage => PageIndex > 1;
}

public class PaginationResponse<T> : ApiResponse<PaginationValue<T>>
{
}