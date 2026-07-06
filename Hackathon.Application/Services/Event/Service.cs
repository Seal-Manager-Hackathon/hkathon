using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.Event;

public class Service : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(IEventRepository eventRepository, IAuthorizationService authorizationService)
    {
        _eventRepository = eventRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetRecentEventsResponse> GetRecentEvents()
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var events = await _eventRepository.GetRecentAsync(5);

        return new GetRecentEventsResponse
        {
            Events = events.Select(e => new EventItem
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Status = e.Status?.ToString(),
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                CreatedAt = e.CreatedAt
            }).ToList()
        };
    }

    public async Task<GetEventCountResponse> GetEventCount(GetEventCountRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        EventStatusEnum? status = null;

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<EventStatusEnum>(request.Status, true, out var parsedStatus))
            {
                throw new BadRequestException("Invalid Status. Must be: Draft, Published, Closed");
            }
            status = parsedStatus;
        }

        var total = await _eventRepository.CountByStatusAsync(status);

        return new GetEventCountResponse
        {
            Total = total
        };
    }
}
