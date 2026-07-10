using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.CriteriaTemplate;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.CriteriaTemplate;

public class Service : ICriteriaTemplateService
{
    private readonly ICriteriaTemplateRepository _criteriaTemplateRepository;
    private readonly ICriteriaItemRepository _criteriaItemRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ICriteriaTemplateRepository criteriaTemplateRepository,
        ICriteriaItemRepository criteriaItemRepository,
        IRoundRepository roundRepository,
        IAuthorizationService authorizationService)
    {
        _criteriaTemplateRepository = criteriaTemplateRepository;
        _criteriaItemRepository = criteriaItemRepository;
        _roundRepository = roundRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(Guid roundId, string? keyword)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var templates = await _criteriaTemplateRepository.GetByRoundIdAsync(roundId);

        // Lecturer chỉ lấy template đang active và không bị disable
        var query = templates
            .Where(t => t.IsActive && !t.IsDisable);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
        }

        var result = query
            .Select(t => new CriteriaTemplateItem
            {
                Id = t.Id,
                RoundId = t.RoundId,
                Title = t.Title,
                Description = t.Description,
                IsDisable = t.IsDisable,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToList();

        return new GetCriteriaTemplatesByRoundResponse
        {
            Templates = result,
            TotalCount = result.Count,
            PageIndex = 1,
            PageSize = result.Count > 0 ? result.Count : 10
        };
    }

    public async Task<GetCriteriaTemplateDetailResponse> GetCriteriaTemplateDetail(Guid templateId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var template = await _criteriaTemplateRepository.GetByIdAsync(templateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return new GetCriteriaTemplateDetailResponse
        {
            Id = template.Id,
            RoundId = template.RoundId,
            Title = template.Title,
            Description = template.Description,
            IsDisable = template.IsDisable,
            IsActive = template.IsActive,
            Items = template.CriteriaItems
                .Where(ci => !ci.IsDisable)
                .Select(ci => new CriteriaTemplateItemDetail
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

    public async Task<GetCriteriaItemsByTemplateResponse> GetCriteriaItemsByTemplate(Guid templateId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var template = await _criteriaTemplateRepository.GetByIdAsync(templateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var items = await _criteriaTemplateRepository.GetItemsByTemplateIdAsync(templateId);
        var activeItems = items
            .Where(i => !i.IsDisable)
            .Select(i => new CriteriaItemInfo
            {
                Id = i.Id,
                CriteriaTemplateId = i.CriteriaTemplateId,
                Name = i.Name,
                Description = i.Description,
                Score = i.Score,
                IsDisable = i.IsDisable,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            })
            .ToList();

        return new GetCriteriaItemsByTemplateResponse
        {
            Items = activeItems,
            TotalCount = activeItems.Count,
            PageIndex = 1,
            PageSize = activeItems.Count > 0 ? activeItems.Count : 10
        };
    }

    public async Task<GetCriteriaItemDetailResponse> GetCriteriaItemDetail(Guid itemId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var item = await _criteriaItemRepository.GetByIdAsync(itemId);
        if (item == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

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
