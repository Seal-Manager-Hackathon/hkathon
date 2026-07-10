using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Round;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.Round;

public class Service : IRoundService
{
    private readonly IRoundRepository _roundRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRoundRepository roundRepository,
        IEventRepository eventRepository,
        IAuthorizationService authorizationService)
    {
        _roundRepository = roundRepository;
        _eventRepository = eventRepository;
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

    public async Task<int?> GetMaxRoundNo(Guid eventId)
    {
        _authorizationService.Authenticate();

        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        return await _roundRepository.GetMaxRoundNoAsync(eventId);
    }
}
