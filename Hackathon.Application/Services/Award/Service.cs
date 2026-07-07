using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Award;

public class Service : IAwardService
{
    private readonly IAwardRepository _awardRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IAwardRepository awardRepository,
        IEventRepository eventRepository,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _awardRepository = awardRepository;
        _eventRepository = eventRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetAwardsResponse> GetAwards(GetAwardsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allAwards = await _awardRepository.GetByEventIdAsync(request.EventId);

        var query = allAwards.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(kw));
        }

        if (request.IsDisable.HasValue)
        {
            query = query.Where(a => a.IsDisable == request.IsDisable.Value);
        }

        var totalCount = query.Count();

        var items = query
            .OrderBy(a => a.LevelAward)
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

    public async Task<CreateAwardResponse> CreateAward(CreateAwardRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var existingAwards = await _awardRepository.GetByEventIdAsync(request.EventId);

        var hasLevelOne = existingAwards.Any(a => a.LevelAward == 1);
        int level;
        if (!hasLevelOne)
        {
            level = 1;
        }
        else
        {
            level = existingAwards.Max(a => a.LevelAward) + 1;
        }

        var now = DateTimeOffset.UtcNow;
        var award = new Awards
        {
            Id = Guid.NewGuid(),
            EventId = request.EventId,
            Name = request.Name,
            Description = request.Description,
            LevelAward = level,
            NumberOfAward = request.NumberOfAward,
            Prize = request.Prize,
            IsDisable = false,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _awardRepository.AddAsync(award);
        await _unitOfWork.SaveChangesAsync();

        return new CreateAwardResponse
        {
            Id = award.Id,
            EventId = award.EventId,
            Name = award.Name,
            Description = award.Description,
            LevelAward = award.LevelAward,
            NumberOfAward = award.NumberOfAward ?? 1,
            Prize = award.Prize ?? 0,
            IsDisable = award.IsDisable
        };
    }

    public async Task DeleteAward(Guid awardId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var award = await _awardRepository.GetByIdAsync(awardId);
        if (award == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var deletedLevel = award.LevelAward;

        award.IsDisable = true;
        award.LevelAward = 0;
        award.UpdatedAt = DateTimeOffset.UtcNow;

        var allAwards = await _awardRepository.GetByEventIdAsync(award.EventId);
        var higherAwards = allAwards
            .Where(a => a.Id != awardId && a.LevelAward > deletedLevel)
            .OrderBy(a => a.LevelAward)
            .ToList();

        foreach (var higher in higherAwards)
        {
            higher.LevelAward--;
            higher.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAward(UpdateAwardRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var award = await _awardRepository.GetByIdAsync(request.AwardId);
        if (award == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        if (request.Name != null)
            award.Name = request.Name;
        if (request.Description != null)
            award.Description = request.Description;
        if (request.NumberOfAward.HasValue)
            award.NumberOfAward = request.NumberOfAward.Value;
        if (request.Prize.HasValue)
            award.Prize = request.Prize.Value;
        if (request.IsDisable.HasValue)
            award.IsDisable = request.IsDisable.Value;

        award.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }
}
