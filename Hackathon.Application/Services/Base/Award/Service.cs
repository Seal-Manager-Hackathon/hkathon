using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Award;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.Award;

public class Service : IAwardService
{
    private readonly IAwardRepository _awardRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IAwardRepository awardRepository,
        IAuthorizationService authorizationService)
    {
        _awardRepository = awardRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetAwardDetailResponse> GetAwardDetail(Guid awardId)
    {
        _authorizationService.Authenticate();

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
}
