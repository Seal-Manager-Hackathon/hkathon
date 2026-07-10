using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.CriteriaTemplate;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.CriteriaTemplate;

public class Service : ICriteriaTemplateService
{
    private readonly ICriteriaTemplateRepository _criteriaTemplateRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ICriteriaTemplateRepository criteriaTemplateRepository,
        IRoundRepository roundRepository,
        IAuthorizationService authorizationService)
    {
        _criteriaTemplateRepository = criteriaTemplateRepository;
        _roundRepository = roundRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(Guid roundId)
    {
        _authorizationService.Authenticate();

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var templates = await _criteriaTemplateRepository.GetByRoundIdAsync(roundId);
        var activeTemplates = templates
            .Where(t => !t.IsDisable)
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
            Templates = activeTemplates,
            TotalCount = activeTemplates.Count,
            PageIndex = 1,
            PageSize = activeTemplates.Count > 0 ? activeTemplates.Count : 10
        };
    }
}
