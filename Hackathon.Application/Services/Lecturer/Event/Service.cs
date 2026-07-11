using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Event;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Event;

public class Service : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IEventRepository eventRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _eventRepository = eventRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetLecturerEventsResponse> GetLecturerEvents(GetLecturerEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        // Parse status enum
        EventStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<EventStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Draft, Published, Closed");
            status = parsed;
        }

        var (items, totalCount) = await _eventRepository.SearchAsync(
            request.Keyword, status,
            request.FromDate, request.ToDate, request.IsDisable,
            request.PageIndex, request.PageSize);

        return new GetLecturerEventsResponse
        {
            Events = items.Select(e => new LecturerEventItem
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Status = e.Status?.ToString(),
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                IsDisable = e.IsDisable,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetLecturerEventsResponse> GetLecturerAssignEvents(GetLecturerEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

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

        var (items, totalCount) = await _assignEventRepository.GetLecturerAssignEventsByUserIdAsync(
            currentUserId.Value, request.Keyword, status,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetLecturerEventsResponse
        {
            Events = items.Select(ae => new LecturerEventItem
            {
                Id = ae.Event.Id,
                Name = ae.Event.Name,
                Description = ae.Event.Description,
                Status = ae.Event.Status?.ToString(),
                StartTime = ae.Event.StartTime,
                EndTime = ae.Event.EndTime,
                IsDisable = ae.Event.IsDisable,
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

    public async Task<GetRecentEventsResponse> GetRecentEvents()
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var (items, _) = await _assignEventRepository.GetLecturerAssignEventsByUserIdAsync(
            currentUserId.Value, null, null,
            null, null, 1, 10);

        return new GetRecentEventsResponse
        {
            Events = items.Select(ae => new EventItem
            {
                Id = ae.Event.Id,
                Name = ae.Event.Name,
                Description = ae.Event.Description,
                Status = ae.Event.Status?.ToString(),
                StartTime = ae.Event.StartTime,
                EndTime = ae.Event.EndTime,
                IsDisable = ae.Event.IsDisable,
                CreatedAt = ae.Event.CreatedAt,
                UpdatedAt = ae.Event.UpdatedAt
            }).ToList()
        };
    }

    public async Task<GetEventCountResponse> GetEventCount(GetEventCountRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        EventStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<EventStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Draft, Published, Closed");
            status = parsed;
        }

        var (_, totalCount) = await _assignEventRepository.GetLecturerAssignEventsByUserIdAsync(
            currentUserId.Value, null, status,
            null, null, 1, int.MaxValue);

        return new GetEventCountResponse
        {
            Total = totalCount
        };
    }

    public async Task<GetLecturerEventDetailResponse> GetLecturerEventDetail(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        var currentUserId = _currentUserService.UserId;
        Guid? eventRoleId = null;
        string? eventRoleName = null;

        if (currentUserId.HasValue)
        {
            var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(eventId, currentUserId.Value);
            if (assignEvent != null)
            {
                eventRoleId = assignEvent.EventRoleId;
                eventRoleName = assignEvent.EventRole?.Name.ToString();
            }
        }

        return new GetLecturerEventDetailResponse
        {
            Id = ev.Id,
            Name = ev.Name,
            Description = ev.Description,
            StartTime = ev.StartTime,
            EndTime = ev.EndTime,
            RegisterLimitTime = ev.RegisterLimitTime,
            LimitTeam = ev.LimitTeam,
            MinMember = ev.MinMember,
            MaxMember = ev.MaxMember,
            Status = ev.Status?.ToString(),
            IsDisable = ev.IsDisable,
            NumberRound = ev.NumberRound,
            Season = ev.Season?.ToString(),
            EventRoleId = eventRoleId,
            EventRoleName = eventRoleName,
            CreatedAt = ev.CreatedAt,
            UpdatedAt = ev.UpdatedAt
        };
    }
}