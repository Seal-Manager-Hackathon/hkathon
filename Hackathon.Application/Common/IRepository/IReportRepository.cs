using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IReportRepository
{
    Task<List<Reports>> GetRecentAsync(int count);
}
