using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Event;

public class Service : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(IEventRepository eventRepository, IRoundRepository roundRepository, IAuthorizationService authorizationService, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _roundRepository = roundRepository;
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
            IsDisable = false,
            NumberRound = 0,
            Season = season,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _eventRepository.AddAsync(ev);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<SetupCheckResponse> IsEventSetupComplete(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        var now = DateTimeOffset.UtcNow;
        var missingFields = new List<string>();

        // Check basic fields
        if (string.IsNullOrWhiteSpace(ev.Name)) missingFields.Add("Name");
        if (!ev.StartTime.HasValue || ev.StartTime <= now) missingFields.Add("StartTime");
        if (!ev.EndTime.HasValue || ev.EndTime <= ev.StartTime) missingFields.Add("EndTime");
        if (!ev.Season.HasValue) missingFields.Add("Season");
        if (string.IsNullOrWhiteSpace(ev.Description)) missingFields.Add("Description");
        if (!ev.LimitTeam.HasValue || ev.LimitTeam <= 0) missingFields.Add("LimitTeam");
        if (!ev.MinMember.HasValue || ev.MinMember <= 0) missingFields.Add("MinMember");
        if (!ev.MaxMember.HasValue || ev.MaxMember <= 0) missingFields.Add("MaxMember");

        // Check có ít nhất 1 round
        var hasRound = await _roundRepository.GetFirstRoundByEventIdAsync(eventId) != null;
        if (!hasRound) missingFields.Add("Round");

        return new SetupCheckResponse
        {
            IsComplete = missingFields.Count == 0,
            MissingFields = missingFields
        };
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

        // IsDisable hoàn toàn độc lập, ko kéo theo bất kỳ logic nào khác
        if (request.IsDisable.HasValue)
            ev.IsDisable = request.IsDisable.Value;

        // Nếu chuyển từ Draft → Published, phải check setup complete
        if (request.Status != null)
        {
            if (!Enum.TryParse<EventStatusEnum>(request.Status, true, out var status))
                throw new BadRequestException("Invalid Status. Must be: Draft, Published, Closed");

            if (status == EventStatusEnum.Published && ev.Status == EventStatusEnum.Draft)
            {
                var setupCheck = await IsEventSetupComplete(request.EventId);
                if (!setupCheck.IsComplete)
                    throw new BadRequestException(
                        $"Cannot Publish Event. Setup Not Complete. Missing: {string.Join(", ", setupCheck.MissingFields)}");
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
                IsDisable = e.IsDisable,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
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
            request.FromDate, request.ToDate, request.IsDisable,
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
                IsDisable = e.IsDisable,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
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

    public async Task DeleteEvent(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        if (ev.IsDisable)
            throw new BadRequestException("Event Is Already Deleted");

        ev.IsDisable = true;
        ev.UpdatedAt = DateTimeOffset.UtcNow;

        await _eventRepository.UpdateAsync(ev);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreEvent(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        if (!ev.IsDisable)
            throw new BadRequestException("Event Is Not Deleted");

        ev.IsDisable = false;
        ev.UpdatedAt = DateTimeOffset.UtcNow;

        await _eventRepository.UpdateAsync(ev);
        await _unitOfWork.SaveChangesAsync();
    }
}
