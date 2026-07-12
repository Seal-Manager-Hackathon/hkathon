using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.CriteriaTemplate;

public class Service : ICriteriaTemplateService
{
    private readonly ICriteriaTemplateRepository _criteriaTemplateRepository;
    private readonly ICriteriaItemRepository _criteriaItemRepository;
    private readonly IRoundRepository _roundRepository;

    public Service(
        ICriteriaTemplateRepository criteriaTemplateRepository,
        IRoundRepository roundRepository,
        ICriteriaItemRepository criteriaItemRepository)
    {
        _criteriaTemplateRepository = criteriaTemplateRepository;
        _roundRepository = roundRepository;
        _criteriaItemRepository = criteriaItemRepository;
    }

    public async Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(GetCriteriaTemplatesByRoundRequest request)
    {
        var round = await _roundRepository.GetByIdAsync(request.RoundId);
        if (round == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Student: only get active and non-disabled templates
        var allTemplates = await _criteriaTemplateRepository.GetByRoundIdAsync(request.RoundId);

        var query = allTemplates
            .Where(t => !t.IsDisable && t.IsActive)
            .AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
        }

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
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
            Templates = items,
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetCriteriaItemsByTemplateResponse> GetCriteriaItemsByTemplate(GetCriteriaItemsByTemplateRequest request)
    {
        var template = await _criteriaTemplateRepository.GetByIdAsync(request.TemplateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allItems = await _criteriaTemplateRepository.GetItemsByTemplateIdAsync(request.TemplateId);

        // Only show non-disabled items
        var query = allItems.Where(i => !i.IsDisable).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(i => i.Name.ToLower().Contains(kw));
        }

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
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
            Items = items,
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetCriteriaTemplateDetailResponse> GetCriteriaTemplateDetail(Guid templateId)
    {
        var template = await _criteriaTemplateRepository.GetByIdAsync(templateId);
        if (template == null || template.IsDisable)
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

    public async Task<GetCriteriaItemDetailResponse> GetCriteriaItemDetail(Guid itemId)
    {
        var item = await _criteriaItemRepository.GetByIdAsync(itemId);
        if (item == null || item.IsDisable)
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
