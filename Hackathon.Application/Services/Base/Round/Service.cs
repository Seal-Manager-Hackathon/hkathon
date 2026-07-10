using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Round;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.Round;

public class Service : IRoundService
{
    private readonly IRoundRepository _roundRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRoundRepository roundRepository,
        IAuthorizationService authorizationService)
    {
        _roundRepository = roundRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetRoundDetailResponse> GetRoundDetail(Guid roundId)
    {
        _authorizationService.Authenticate();

        var round = await _roundRepository.GetDetailByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return new GetRoundDetailResponse
        {
            Id = round.Id,
            EventId = round.EventId,
            EventName = round.Event?.Name,
            Name = round.Name,
            Description = round.Description,
            RoundNo = round.RoundNo,
            StartTime = round.StartTime,
            EndTime = round.EndTime,
            StartSubmission = round.StartSubmission,
            EndSubmission = round.EndSubmission,
            LimitTeam = round.LimitTeam,
            IsDisable = round.IsDisable,
            CreatedAt = round.CreatedAt,
            UpdatedAt = round.UpdatedAt
        };
    }
}
