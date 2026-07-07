using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Event;

public class Service : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(IEventRepository eventRepository, IAuthorizationService authorizationService, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateEvent(CreateEventRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var now = DateTimeOffset.UtcNow;

        // Validate thời gian
        if (request.StartTime <= now)
            throw new BadRequestException(ErrMsg.Event.StartTimeMustBeAfterNow);

        if (request.EndTime <= request.StartTime)
            throw new BadRequestException(ErrMsg.Event.EndTimeMustBeAfterStartTime);

        if (request.RegisterLimitTime.HasValue)
        {
            if (request.RegisterLimitTime.Value <= request.StartTime)
                throw new BadRequestException(ErrMsg.Event.RegisterLimitTimeMustBeAfterStartTime);

            if (request.RegisterLimitTime.Value >= request.EndTime)
                throw new BadRequestException(ErrMsg.Event.RegisterLimitTimeMustBeBeforeEndTime);
        }

        SeasonEnum? season = null;
        if (!string.IsNullOrWhiteSpace(request.Season))
        {
            if (!Enum.TryParse<SeasonEnum>(request.Season, true, out var parsed))
                throw new BadRequestException("Invalid Season. Must be: Spring, Summer, Autumn, Winter");
            season = parsed;
        }

        var ev = new Events
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            RegisterLimitTime = request.RegisterLimitTime,
            LimitTeam = request.LimitTeam,
            MinMember = request.MinMember,
            MaxMember = request.MaxMember,
            Status = EventStatusEnum.Draft,
            IsDisable = true,
            NumberRound = 0,
            Season = season,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _eventRepository.AddAsync(ev);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateEvent(UpdateEventRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        var now = DateTimeOffset.UtcNow;

        // Lấy giá trị để validate — ưu tiên request, nếu null lấy từ entity
        var startTime = request.StartTime ?? ev.StartTime;
        var endTime = request.EndTime ?? ev.EndTime;
        var registerLimitTime = request.RegisterLimitTime ?? ev.RegisterLimitTime;

        if (startTime <= now && request.StartTime.HasValue)
            throw new BadRequestException(ErrMsg.Event.StartTimeMustBeAfterNow);

        if (endTime <= startTime && (request.EndTime.HasValue || request.StartTime.HasValue))
            throw new BadRequestException(ErrMsg.Event.EndTimeMustBeAfterStartTime);

        if (registerLimitTime.HasValue)
        {
            if (registerLimitTime.Value <= startTime && request.RegisterLimitTime.HasValue)
                throw new BadRequestException(ErrMsg.Event.RegisterLimitTimeMustBeAfterStartTime);

            if (registerLimitTime.Value >= endTime && request.RegisterLimitTime.HasValue)
                throw new BadRequestException(ErrMsg.Event.RegisterLimitTimeMustBeBeforeEndTime);
        }

        // Update fields
        if (request.Name != null)
            ev.Name = request.Name;
        if (request.Description != null)
            ev.Description = request.Description;
        if (request.StartTime.HasValue)
            ev.StartTime = request.StartTime.Value;
        if (request.EndTime.HasValue)
            ev.EndTime = request.EndTime.Value;
        if (request.RegisterLimitTime.HasValue)
            ev.RegisterLimitTime = request.RegisterLimitTime;
        if (request.LimitTeam.HasValue)
            ev.LimitTeam = request.LimitTeam;
        if (request.MinMember.HasValue)
            ev.MinMember = request.MinMember;
        if (request.MaxMember.HasValue)
            ev.MaxMember = request.MaxMember;
        if (request.Season != null)
        {
            if (!Enum.TryParse<SeasonEnum>(request.Season, true, out var season))
                throw new BadRequestException("Invalid Season. Must be: Spring, Summer, Autumn, Winter");
            ev.Season = season;
        }

        // Check disable change: true → false
        // Nếu request.IsDisable == false (muốn mở), phải đủ thông tin và status không được là Draft
        if (request.IsDisable.HasValue && !request.IsDisable.Value && ev.IsDisable)
        {
            var now2 = DateTimeOffset.UtcNow; // refresh cho check publish
            var missingFields = new List<string>();
            if (string.IsNullOrWhiteSpace(ev.Name)) missingFields.Add("Name");
            if (!ev.StartTime.HasValue || ev.StartTime <= now2) missingFields.Add("StartTime");
            if (!ev.EndTime.HasValue || ev.EndTime <= ev.StartTime) missingFields.Add("EndTime");
            if (!ev.Season.HasValue) missingFields.Add("Season");

            if (missingFields.Count > 0)
                throw new BadRequestException($"Cannot Enable Event. Missing Required Fields: {string.Join(", ", missingFields)}");

            if (ev.Status == EventStatusEnum.Draft)
                ev.Status = EventStatusEnum.Published;
        }

        if (request.IsDisable.HasValue)
            ev.IsDisable = request.IsDisable.Value;

        // Nếu chuyển từ Draft → Published, check thông tin đã đầy đủ chưa
        if (request.Status != null)
        {
            if (!Enum.TryParse<EventStatusEnum>(request.Status, true, out var status))
                throw new BadRequestException("Invalid Status. Must be: Draft, Published, Closed");

            if (status == EventStatusEnum.Published && ev.Status == EventStatusEnum.Draft)
            {
                var missingFields = new List<string>();
                if (string.IsNullOrWhiteSpace(ev.Name)) missingFields.Add("Name");
                if (!ev.StartTime.HasValue || ev.StartTime <= now) missingFields.Add("StartTime");
                if (!ev.EndTime.HasValue || ev.EndTime <= ev.StartTime) missingFields.Add("EndTime");
                if (!ev.Season.HasValue) missingFields.Add("Season");

                if (missingFields.Count > 0)
                    throw new BadRequestException($"Cannot Publish Event. Missing Required Fields: {string.Join(", ", missingFields)}");
            }

            ev.Status = status;
        }

        ev.UpdatedAt = now;
        await _eventRepository.UpdateAsync(ev);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetRecentEventsResponse> GetRecentEvents()
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var events = await _eventRepository.GetRecentAsync(10);

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

    public async Task<GetEventsResponse> GetEvents(GetEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        EventStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<EventStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Draft, Published, Closed");
            status = parsed;
        }

        var (items, totalCount) = await _eventRepository.SearchAsync(
            request.Keyword, status,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetEventsResponse
        {
            Events = items.Select(e => new EventItem
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Status = e.Status?.ToString(),
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                CreatedAt = e.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetEventDetailResponse> GetEventDetail(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        return new GetEventDetailResponse
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
            CreatedAt = ev.CreatedAt,
            UpdatedAt = ev.UpdatedAt
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
