using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Report;

namespace Hackathon.Application.Common.IRepository;

public interface IReportRepository
{
    Task<List<Reports>> GetRecentAsync(int count);
    Task<Reports?> GetByIdAsync(Guid reportId);
    Task<(List<Reports> Items, int TotalCount)> SearchAsync(
        string? keyword, ReportStatusEnum? status,
        int pageIndex, int pageSize);
    Task AddAsync(Reports report);
    Task<(List<Reports> Items, int TotalCount)> GetByUserIdAsync(
        Guid userId, string? keyword, ReportStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize);
}
