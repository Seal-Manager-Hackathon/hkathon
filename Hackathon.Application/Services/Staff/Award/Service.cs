using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Award;

public class Service : IAwardService
{
    private readonly IAwardRepository _awardRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IAwardRepository awardRepository,
        IEventRepository eventRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _awardRepository = awardRepository;
        _eventRepository = eventRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    private async Task EnsureStaffAssignedToEvent(Guid eventId)
    {
        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);
    }

    public async Task<GetAwardsResponse> GetAwards(Guid eventId, GetAwardsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var eventEntity = await _eventRepository.GetByIdAsync(eventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Staff phải được assign vào event này
        await EnsureStaffAssignedToEvent(eventId);

        var allAwards = await _awardRepository.GetByEventIdAsync(eventId);

        // Staff chỉ thấy award IsDisable == false
        var query = allAwards.Where(a => !a.IsDisable).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(kw));
        }

        var totalCount = query.Count();

        var items = query
            .OrderBy(a => a.LevelAward == 0)
            .ThenBy(a => a.LevelAward)
            .ThenByDescending(a => a.Prize)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new AwardItem
            {
                Id = a.Id,
                EventId = a.EventId,
                Name = a.Name,
                Description = a.Description,
                LevelAward = a.LevelAward,
                NumberOfAward = a.NumberOfAward,
                Prize = a.Prize,
                IsDisable = a.IsDisable,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .ToList();

        return new GetAwardsResponse
        {
            Awards = items,
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetAwardDetailResponse> GetAwardDetail(Guid awardId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var award = await _awardRepository.GetByIdAsync(awardId);
        if (award == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Staff không xem được award đã disable
        if (award.IsDisable)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Staff phải được assign vào event này
        await EnsureStaffAssignedToEvent(award.EventId);

        return new GetAwardDetailResponse
        {
            Id = award.Id,
            EventId = award.EventId,
            Name = award.Name,
            Description = award.Description,
            LevelAward = award.LevelAward,
            NumberOfAward = award.NumberOfAward,
            Prize = award.Prize,
            IsDisable = award.IsDisable,
            CreatedAt = award.CreatedAt,
            UpdatedAt = award.UpdatedAt
        };
    }
}