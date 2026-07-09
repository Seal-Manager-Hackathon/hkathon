using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Round;

public class Service : IRoundService
{
    private readonly IRoundRepository _roundRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRoundRepository roundRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _roundRepository = roundRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetRoundsResponse> GetRounds(Guid eventId, GetRoundsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            eventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var (items, totalCount) = await _roundRepository.SearchByEventIdAsync(
            eventId, request.Keyword, request.RoundNo, isDisable: false,
            request.PageIndex, request.PageSize);

        return new GetRoundsResponse
        {
            Rounds = items.Select(r => new StaffRoundItem
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

    public async Task<GetRoundDetailResponse> GetRoundDetail(Guid eventId, Guid roundId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            eventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var round = await _roundRepository.GetDetailByIdAsync(roundId);
        if (round == null || round.IsDisable)
            throw new NotFoundException("Round Not Found");

        return new GetRoundDetailResponse
        {
            Id = round.Id,
            EventId = round.EventId,
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
