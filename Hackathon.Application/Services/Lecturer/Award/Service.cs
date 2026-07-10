using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Award;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Award;

public class Service : IAwardService
{
    private readonly IAwardRepository _awardRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IAwardRepository awardRepository,
        IEventRepository eventRepository,
        IAuthorizationService authorizationService)
    {
        _awardRepository = awardRepository;
        _eventRepository = eventRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetAwardsResponse> GetAwards(Guid eventId, string? keyword)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var eventEntity = await _eventRepository.GetByIdAsync(eventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allAwards = await _awardRepository.GetByEventIdAsync(eventId);

        // Lecturer chỉ lấy award không bị disable
        var query = allAwards.Where(a => !a.IsDisable);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(kw));
        }

        var items = query
            .OrderBy(a => a.LevelAward)
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
            TotalCount = items.Count,
            PageIndex = 1,
            PageSize = items.Count > 0 ? items.Count : 10
        };
    }
}
