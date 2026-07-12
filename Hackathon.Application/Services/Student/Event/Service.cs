using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Event;

public class Service : IEventService
{
    private readonly IEventRepository _eventRepository;

    public Service(
        IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<GetEventsResponse> GetEvents(GetEventsRequest request)
    {
        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        EventStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<EventStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Draft, Published, Closed");
            if (parsed == EventStatusEnum.Draft)
                throw new BadRequestException("Invalid Status");
            status = parsed;
        }

        List<Events> allItems;
        int totalCount;

        if (status.HasValue)
        {
            // Khi có status filter, DB query đã loại Draft, ko cần post-filter
            var (items, count) = await _eventRepository.SearchAsync(
                request.Keyword, status,
                request.FromDate, request.ToDate, false,
                request.PageIndex, request.PageSize);
            allItems = items;
            totalCount = count;
        }
        else
        {
            // Ko có status filter: lấy tất cả non-disabled, filter Draft trong memory, rồi phân trang
            var (items, count) = await _eventRepository.SearchAsync(
                request.Keyword, null,
                request.FromDate, request.ToDate, false,
                1, int.MaxValue);
            var filtered = items.Where(e => e.Status != EventStatusEnum.Draft).ToList();
            totalCount = filtered.Count;
            allItems = filtered
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
        }

        return new GetEventsResponse
        {
            Events = allItems.Select(e => new EventItem
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
        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null || ev.IsDisable || ev.Status == EventStatusEnum.Draft)
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
        EventStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<EventStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Draft, Published, Closed");
            if (parsed == EventStatusEnum.Draft)
                throw new BadRequestException("Invalid Status");
            status = parsed;
        }

        // Student: only count non-disabled, non-Draft events
        var (items, _) = await _eventRepository.SearchAsync(
            null, status, null, null, false, 1, int.MaxValue);

        var total = status.HasValue
            ? items.Count
            : items.Count(e => e.Status != EventStatusEnum.Draft);

        return new GetEventCountResponse
        {
            Total = total
        };
    }

    public async Task<GetRecentEventsResponse> GetRecentEvents()
    {
        var events = await _eventRepository.GetRecentAsync(10);

        var filtered = events.Where(e => e.Status != EventStatusEnum.Draft && !e.IsDisable).ToList();

        return new GetRecentEventsResponse
        {
            Events = filtered.Select(e => new EventItem
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
}
