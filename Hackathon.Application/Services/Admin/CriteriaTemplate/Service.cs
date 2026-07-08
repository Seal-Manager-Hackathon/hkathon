using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.CriteriaTemplate;

public class Service : ICriteriaTemplateService
{
    private readonly ICriteriaTemplateRepository _criteriaTemplateRepository;
    private readonly ICriteriaItemRepository _criteriaItemRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        ICriteriaTemplateRepository criteriaTemplateRepository,
        IRoundRepository roundRepository,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork,
        ICriteriaItemRepository criteriaItemRepository)
    {
        _criteriaTemplateRepository = criteriaTemplateRepository;
        _roundRepository = roundRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
        _criteriaItemRepository = criteriaItemRepository;
    }

    public async Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(GetCriteriaTemplatesByRoundRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var round = await _roundRepository.GetByIdAsync(request.RoundId);
        if (round == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allTemplates = await _criteriaTemplateRepository.GetByRoundIdAsync(request.RoundId);

        var query = allTemplates.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
        }

        if (request.IsDisable.HasValue)
            query = query.Where(t => t.IsDisable == request.IsDisable.Value);

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
        _authorizationService.Authorize(RoleEnum.Admin);

        var template = await _criteriaTemplateRepository.GetByIdAsync(request.TemplateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allItems = await _criteriaTemplateRepository.GetItemsByTemplateIdAsync(request.TemplateId);

        var query = allItems.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(i => i.Name.ToLower().Contains(kw));
        }

        if (request.IsDisable.HasValue)
            query = query.Where(i => i.IsDisable == request.IsDisable.Value);

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
        _authorizationService.Authorize(RoleEnum.Admin);

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
        _authorizationService.Authorize(RoleEnum.Admin);

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

    public async Task ActivateCriteriaTemplate(Guid templateId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var template = await _criteriaTemplateRepository.GetByIdAsync(templateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        if (template.IsDisable)
            throw new BadRequestException("Cannot Activate A Deleted Template");

        // Tìm tất cả template cùng round, tắt hết -> chỉ cái được chọn mới active
        var templatesInRound = await _criteriaTemplateRepository.GetByRoundIdAsync(template.RoundId);
        var now = DateTimeOffset.UtcNow;

        foreach (var t in templatesInRound)
        {
            if (t.Id == templateId)
            {
                if (t.IsActive)
                    throw new BadRequestException("This Template Is Already Active");

                t.IsActive = true;
            }
            else
            {
                if (t.IsActive)
                {
                    t.IsActive = false;
                }
            }
            t.UpdatedAt = now;
            await _criteriaTemplateRepository.UpdateAsync(t);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateCriteriaTemplate(CreateCriteriaTemplateRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var round = await _roundRepository.GetByIdAsync(request.RoundId);
        if (round == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var now = DateTimeOffset.UtcNow;
        var template = new CriteriaTemplates
        {
            Id = Guid.NewGuid(),
            RoundId = request.RoundId,
            Title = request.Title,
            Description = request.Description,
            IsDisable = false,
            CreatedAt = now,
            UpdatedAt = now
        };

        template.CriteriaItems = request.Items.Select(item => new CriteriaItems
        {
            Id = Guid.NewGuid(),
            CriteriaTemplateId = template.Id,
            Name = item.Name,
            Description = item.Description,
            Score = item.Score,
            IsDisable = false,
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();

        await _criteriaTemplateRepository.AddAsync(template);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCriteriaTemplate(UpdateCriteriaTemplateRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var template = await _criteriaTemplateRepository.GetByIdAsync(request.TemplateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        if (request.Title != null)
            template.Title = request.Title;
        if (request.Description != null)
            template.Description = request.Description;
        if (request.IsDisable.HasValue)
            template.IsDisable = request.IsDisable.Value;

        template.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateCriteriaItem(Guid templateId, CreateCriteriaItemRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var template = await _criteriaTemplateRepository.GetByIdAsync(templateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var now = DateTimeOffset.UtcNow;
        var item = new CriteriaItems
        {
            Id = Guid.NewGuid(),
            CriteriaTemplateId = templateId,
            Name = request.Name,
            Description = request.Description,
            Score = request.Score,
            IsDisable = false,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _criteriaItemRepository.AddAsync(item);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCriteriaItem(UpdateCriteriaItemRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var item = await _criteriaItemRepository.GetByIdAsync(request.ItemId);
        if (item == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        if (request.Name != null)
            item.Name = request.Name;
        if (request.Description != null)
            item.Description = request.Description;
        if (request.Score.HasValue)
            item.Score = request.Score.Value;
        if (request.IsDisable.HasValue)
            item.IsDisable = request.IsDisable.Value;

        item.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCriteriaItem(Guid itemId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var item = await _criteriaItemRepository.GetByIdAsync(itemId);
        if (item == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        item.IsDisable = true;
        item.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreCriteriaItem(Guid itemId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var item = await _criteriaItemRepository.GetByIdAsync(itemId);
        if (item == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        item.IsDisable = false;
        item.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCriteriaTemplate(Guid templateId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var template = await _criteriaTemplateRepository.GetByIdAsync(templateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        template.IsDisable = true;
        template.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreCriteriaTemplate(Guid templateId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var template = await _criteriaTemplateRepository.GetByIdAsync(templateId);
        if (template == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        template.IsDisable = false;
        template.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }
}
