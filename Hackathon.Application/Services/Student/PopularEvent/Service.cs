using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Enums.RegisterTeam;

namespace Hackathon.Application.Services.Student.PopularEvent;

public class Service : IPopularEventService
{
    private readonly IEventRepository _eventRepository;

    public Service(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<GetPopularEventsResponse> GetPopularEvents(GetPopularEventsRequest request)
    {
        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _eventRepository.GetPublicEventsAsync(request.PageIndex, request.PageSize);

        return new GetPopularEventsResponse
        {
            Events = items.Select(e => new PopularEventItem
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                RegisterLimitTime = e.RegisterLimitTime,
                LimitTeam = e.LimitTeam,
                MinMember = e.MinMember,
                MaxMember = e.MaxMember,
                Status = e.Status?.ToString(),
                NumberRound = e.NumberRound,
                Season = e.Season?.ToString(),
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                ApprovedRegisterTeamCount = e.RegisterTeams?.Count(rt => rt.Status == RegisterTeamStatusEnum.Approved && !rt.IsDisable) ?? 0
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
