using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.CriteriaTemplate;

public class Service : ICriteriaTemplateService
{
    private readonly ICriteriaTemplateRepository _criteriaTemplateRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ICriteriaTemplateRepository criteriaTemplateRepository,
        IRoundRepository roundRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _criteriaTemplateRepository = criteriaTemplateRepository;
        _roundRepository = roundRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetCriteriaTemplateResponse> GetCriteriaTemplateByRoundId(Guid eventId, Guid roundId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            eventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null || round.IsDisable)
            throw new NotFoundException("Round Not Found");

        var templates = await _criteriaTemplateRepository.GetByRoundIdAsync(roundId);
        var activeTemplates = templates.Where(t => !t.IsDisable).ToList();

        return new GetCriteriaTemplateResponse
        {
            Items = activeTemplates.Select(t => new CriteriaTemplateItem
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

    public async Task<GetCriteriaItemsResponse> GetCriteriaItemsByTemplateId(Guid eventId, Guid criteriaTemplateId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            eventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var template = await _criteriaTemplateRepository.GetByIdWithItemsAsync(criteriaTemplateId);
        if (template == null)
            throw new NotFoundException("Criteria Template Not Found");

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
}
