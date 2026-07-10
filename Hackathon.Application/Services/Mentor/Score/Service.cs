using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Score;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Mentor.Score;

public class Service : IScoreService
{
    private readonly IRoundRepository _roundRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRoundRepository roundRepository,
        IRegisterTeamRepository registerTeamRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _roundRepository = roundRepository;
        _registerTeamRepository = registerTeamRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetTeamRoundScoreResponse> GetTeamRoundScore(Guid roundId, Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Get register team to find event & track
        var registerTeam = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (registerTeam == null || registerTeam.IsDisable)
            throw new NotFoundException("Register Team Not Found");

        // Verify mentor is assigned to this event
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(registerTeam.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        // Verify mentor is assigned to this track
        var hasTrackAccess = assignEvent.AssignTracks
            .Any(at => at.TrackId == registerTeam.TrackId && !at.IsDisable);
        if (!hasTrackAccess)
            throw new ForbiddenException("You Are Not Assigned to This Track");

        // Get round detail with scores
        var roundDetail = await _roundRepository.GetRoundDetailWithScoresAsync(roundId, registerTeamId);
        if (roundDetail == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var round = roundDetail.Round;

        // scopeScore = SUM(Scores.TotalScore) của submission cuối cùng
        var lastSubmission = SubmissionHelper.GetLastSubmission(roundDetail);

        var scopeScore = lastSubmission != null
            ? Math.Round(
                lastSubmission.Scores
                    .Where(s => s.TotalScore.HasValue)
                    .Sum(s => s.TotalScore!.Value),
                2)
            : 0m;

        return new GetTeamRoundScoreResponse
        {
            RoundId = roundId,
            RegisterTeamId = registerTeamId,
            EventId = round.EventId,
            EventName = round.Event?.Name ?? "",
            TrackId = registerTeam.TrackId,
            TrackTitle = registerTeam.Track?.Title,
            TopicId = registerTeam.TopicId,
            TopicTitle = registerTeam.Topic?.Title,
            TotalScore = scopeScore,
            SubmissionId = lastSubmission?.Id,
            SubmittedAt = lastSubmission?.SubmittedAt,
            IsLastSubmission = lastSubmission != null
        };
    }
}
