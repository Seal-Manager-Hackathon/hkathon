using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Award;

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
        _authorizationService.Authorize(RoleEnum.Admin);

        var award = await _awardRepository.GetByIdAsync(awardId);
        if (award == null)
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

    public async Task<CreateAwardResponse> CreateAward(CreateAwardRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var existingAwards = await _awardRepository.GetByEventIdAsync(request.EventId);

        // Check duplicate name trong cùng event
        if (existingAwards.Any(a => a.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            throw new BadRequestException("Award Name Already Exists In This Event");

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

    public async Task SwapAwardLevel(Guid awardId, int targetLevel)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        if (targetLevel < 1)
            throw new BadRequestException("Target Level Must Be Greater Than 0");

        var currentAward = await _awardRepository.GetByIdAsync(awardId);
        if (currentAward == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Ko cho swap với award bị disable
        if (currentAward.IsDisable || currentAward.LevelAward == 0)
            throw new BadRequestException("Cannot Swap A Deleted Award");

        // Ko cho swap với chính nó
        if (currentAward.LevelAward == targetLevel)
            throw new BadRequestException("Cannot Swap Award With Itself");

        // Tìm award có level cần đổi
        var allAwards = await _awardRepository.GetByEventIdAsync(currentAward.EventId);
        var targetAward = allAwards.FirstOrDefault(a => a.LevelAward == targetLevel && !a.IsDisable);
        if (targetAward == null)
            throw new BadRequestException("Target Level Not Found In This Event");

        // Swap LevelAward
        (currentAward.LevelAward, targetAward.LevelAward) = (targetAward.LevelAward, currentAward.LevelAward);

        var now = DateTimeOffset.UtcNow;
        currentAward.UpdatedAt = now;
        targetAward.UpdatedAt = now;

        await _awardRepository.UpdateAsync(currentAward);
        await _awardRepository.UpdateAsync(targetAward);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreAward(Guid awardId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var award = await _awardRepository.GetByIdAsync(awardId);
        if (award == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Lấy level cao nhất trong cùng event + 1
        var allAwards = await _awardRepository.GetByEventIdAsync(award.EventId);
        int maxLevel = allAwards
            .Where(a => a.Id != awardId && !a.IsDisable)
            .Select(a => (int?)a.LevelAward)
            .Max() ?? 0;

        award.IsDisable = false;
        award.LevelAward = maxLevel + 1;
        award.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
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
