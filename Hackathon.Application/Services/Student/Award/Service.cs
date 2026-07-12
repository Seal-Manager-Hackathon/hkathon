using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Award;

public class Service : IAwardService
{
    private readonly IAwardRepository _awardRepository;
    private readonly IEventRepository _eventRepository;

    public Service(
        IAwardRepository awardRepository,
        IEventRepository eventRepository)
    {
        _awardRepository = awardRepository;
        _eventRepository = eventRepository;
    }

    public async Task<GetAwardsResponse> GetAwards(GetAwardsRequest request)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allAwards = await _awardRepository.GetByEventIdAsync(request.EventId);

        // Student: chi lay award khong bi disable
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
        var award = await _awardRepository.GetByIdAsync(awardId);
        if (award == null || award.IsDisable)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

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
