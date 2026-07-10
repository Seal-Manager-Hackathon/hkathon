using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.CriteriaTemplate;

public class Service : ICriteriaTemplateService
{
    private readonly ICriteriaTemplateRepository _criteriaTemplateRepository;
    private readonly ICriteriaItemRepository _criteriaItemRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ICriteriaTemplateRepository criteriaTemplateRepository,
        ICriteriaItemRepository criteriaItemRepository,
        IRoundRepository roundRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _criteriaTemplateRepository = criteriaTemplateRepository;
        _criteriaItemRepository = criteriaItemRepository;
        _roundRepository = roundRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetCriteriaTemplateResponse> GetCriteriaTemplateByRoundId(Guid roundId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null || round.IsDisable)
            throw new NotFoundException("Round Not Found");

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            round.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var templates = await _criteriaTemplateRepository.GetByRoundIdAsync(roundId);
        var activeTemplates = templates
            .Where(t => !t.IsDisable)
            .OrderByDescending(t => t.IsActive)
            .ThenByDescending(t => t.CreatedAt)
            .ToList();

        return new GetCriteriaTemplateResponse
        {
            Templates = activeTemplates.Select(t => new CriteriaTemplateItem
            {
                Id = t.Id,
                RoundId = t.RoundId,
                Title = t.Title,
                Description = t.Description,
                IsDisable = t.IsDisable,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList(),
            TotalCount = activeTemplates.Count,
            PageIndex = 1,
            PageSize = activeTemplates.Count > 0 ? activeTemplates.Count : 10
        };
    }

    public async Task<GetCriteriaItemsResponse> GetCriteriaItemsByTemplateId(Guid criteriaTemplateId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var template = await _criteriaTemplateRepository.GetByIdWithItemsAsync(criteriaTemplateId);
        if (template == null)
            throw new NotFoundException("Criteria Template Not Found");

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Get round to find EventId for assignment check
        var round = await _roundRepository.GetByIdAsync(template.RoundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            round.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var items = template.CriteriaItems.Where(ci => !ci.IsDisable).ToList();

        return new GetCriteriaItemsResponse
        {
            Items = items.Select(ci => new CriteriaItemDetail
            {
                Id = ci.Id,
                CriteriaTemplateId = ci.CriteriaTemplateId,
                Name = ci.Name,
                Description = ci.Description,
                Score = ci.Score,
                IsDisable = ci.IsDisable,
                CreatedAt = ci.CreatedAt,
                UpdatedAt = ci.UpdatedAt
            }).ToList(),
            TotalCount = items.Count,
            PageIndex = 1,
            PageSize = items.Count > 0 ? items.Count : 10
        };
    }

    public async Task<GetCriteriaTemplateDetailResponse> GetCriteriaTemplateDetail(Guid templateId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var template = await _criteriaTemplateRepository.GetByIdAsync(templateId);
        if (template == null)
            throw new NotFoundException("Criteria Template Not Found");

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Get round to find EventId for assignment check
        var round = await _roundRepository.GetByIdAsync(template.RoundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            round.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        return new GetCriteriaTemplateDetailResponse
        {
            Id = template.Id,
            RoundId = template.RoundId,
            Title = template.Title,
            Description = template.Description,
            IsDisable = template.IsDisable,
            IsActive = template.IsActive,
            Items = template.CriteriaItems.Select(ci => new CriteriaTemplateItemDetail
            {
                Id = ci.Id,
                Name = ci.Name,
                Description = ci.Description,
                Score = ci.Score,
                IsDisable = ci.IsDisable,
                CreatedAt = ci.CreatedAt
            }).ToList(),
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt
        };
    }

    public async Task<GetCriteriaItemDetailResponse> GetCriteriaItemDetail(Guid itemId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var item = await _criteriaItemRepository.GetByIdAsync(itemId);
        if (item == null)
            throw new NotFoundException("Criteria Item Not Found");

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Get template → round → EventId for assignment check
        var template = await _criteriaTemplateRepository.GetByIdAsync(item.CriteriaTemplateId);
        if (template == null)
            throw new NotFoundException("Criteria Template Not Found");

        var round = await _roundRepository.GetByIdAsync(template.RoundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            round.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        return new GetCriteriaItemDetailResponse
        {
            Id = item.Id,
            CriteriaTemplateId = item.CriteriaTemplateId,
            Name = item.Name,
            Description = item.Description,
            Score = item.Score,
            IsDisable = item.IsDisable,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}
