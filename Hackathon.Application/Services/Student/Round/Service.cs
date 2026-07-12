using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Round;

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

    public async Task<GetRoundsResponse> GetRounds(GetRoundsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        // Student: only see rounds that are not disabled
        var (items, totalCount) = await _roundRepository.SearchByEventIdAsync(
            request.EventId, request.Keyword, request.RoundNo, false,
            request.PageIndex, request.PageSize);

        return new GetRoundsResponse
        {
            Rounds = items.Select(r => new RoundItem
            {
                Id = r.Id,
                EventId = r.EventId,
                Name = r.Name,
                Description = r.Description,
                RoundNo = r.RoundNo,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                StartSubmission = r.StartSubmission,
                EndSubmission = r.EndSubmission,
                LimitTeam = r.LimitTeam,
                IsDisable = r.IsDisable,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetRoundDetailResponse> GetRoundDetail(Guid roundId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var round = await _roundRepository.GetDetailByIdAsync(roundId);
        if (round == null || round.IsDisable)
            throw new NotFoundException(ErrMsg.Round.RoundNoNotFound);

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
