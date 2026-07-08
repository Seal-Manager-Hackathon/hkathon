using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Event;

public class Service : IEventService
{
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetMyEventsResponse> GetMyEvents(GetMyEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Parse status enum
        EventStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status)
            && Enum.TryParse<EventStatusEnum>(request.Status, true, out var parsedStatus))
        {
            status = parsedStatus;
        }

        var (items, totalCount) = await _assignEventRepository.GetEventsByStaffUserIdAsync(
            currentUserId.Value, request.Keyword, status,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetMyEventsResponse
        {
            Items = items.Select(ae => new StaffEventItem
            {
                Id = ae.Event.Id,
                Name = ae.Event.Name,
                Description = ae.Event.Description,
                Status = ae.Event.Status?.ToString(),
                NumberRound = ae.Event.NumberRound,
                Season = ae.Event.Season?.ToString(),
                StartTime = ae.Event.StartTime,
                EndTime = ae.Event.EndTime,
                EventRoleId = ae.EventRoleId,
                EventRoleName = ae.EventRole?.Name.ToString(),
                CreatedAt = ae.Event.CreatedAt,
                UpdatedAt = ae.Event.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetMyEventDetailResponse> GetMyEventDetail(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithEventAsync(
            eventId, currentUserId.Value);

        if (assignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");

        var ev = assignEvent.Event;

        return new GetMyEventDetailResponse
        {
            Id = ev.Id,
            Name = ev.Name,
            Description = ev.Description,
            Status = ev.Status?.ToString(),
            NumberRound = ev.NumberRound,
            Season = ev.Season?.ToString(),
            StartTime = ev.StartTime,
            EndTime = ev.EndTime,
            RegisterLimitTime = ev.RegisterLimitTime,
            LimitTeam = ev.LimitTeam,
            MinMember = ev.MinMember,
            MaxMember = ev.MaxMember,
            EventRoleId = assignEvent.EventRoleId,
            EventRoleName = assignEvent.EventRole?.Name.ToString(),
            CreatedAt = ev.CreatedAt,
            UpdatedAt = ev.UpdatedAt
        };
    }
}
