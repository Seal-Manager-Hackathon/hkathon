using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IMentorNotificationRepository
{
    Task<MentorNotifications?> GetByIdAsync(Guid id);
    Task<List<MentorNotifications>> GetByAssignTrackIdAsync(Guid assignTrackId, int pageIndex, int pageSize);
    Task<(List<MentorNotifications> Items, int TotalCount)> GetByAssignTrackIdsAsync(List<Guid> assignTrackIds, int pageIndex, int pageSize);
    Task<int> CountByAssignTrackIdAsync(Guid assignTrackId);
    Task AddAsync(MentorNotifications notification);
    Task UpdateAsync(MentorNotifications notification);
}
