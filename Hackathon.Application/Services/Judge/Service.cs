using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EventRole;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Judge;

public class Service : IJudgeService
{
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IScoreRepository _scoreRepository;
    private readonly ICriteriaTemplateRepository _criteriaTemplateRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IAssignEventRepository assignEventRepository,
        ITrackRepository trackRepository,
        IEventRepository eventRepository,
        ISubmissionRepository submissionRepository,
        IScoreRepository scoreRepository,
        ICriteriaTemplateRepository criteriaTemplateRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _assignEventRepository = assignEventRepository;
        _trackRepository = trackRepository;
        _eventRepository = eventRepository;
        _submissionRepository = submissionRepository;
        _scoreRepository = scoreRepository;
        _criteriaTemplateRepository = criteriaTemplateRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    private Guid GetCurrentUserId()
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);
        var userId = _currentUserService.UserId;
        if (!userId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);
        return userId.Value;
    }

    private async Task ValidateJudgeEventAssignment(Guid userId, Guid eventId)
    {
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(eventId, userId);
        if (assignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");
        if (assignEvent.EventRole == null || assignEvent.EventRole.Name != EventRoleEnum.Judge)
            throw new ForbiddenException("You Are Not Assigned as Judge for This Event");
    }

    private async Task<AssignTracks> ValidateJudgeTrackAssignment(Guid userId, Guid eventId, Guid trackId)
    {
        var assignTrack = await _assignEventRepository.GetGraderAssignTrackAsync(userId, eventId, trackId);
        if (assignTrack == null)
            throw new ForbiddenException("You Are Not Assigned as Judge for This Track");
        return assignTrack;
    }

    public async Task<List<JudgeTrackItem>> GetMyTracks(Guid eventId)
    {
        var currentUserId = GetCurrentUserId();

        await ValidateJudgeEventAssignment(currentUserId, eventId);

        var fullAssignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(
            eventId, currentUserId);
        if (fullAssignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");

        return fullAssignEvent.AssignTracks
            .Where(at => !at.IsDisable && !at.Track.IsDisable)
            .Select(at => new JudgeTrackItem
            {
                AssignTrackId = at.Id,
                TrackId = at.TrackId,
                TrackTitle = at.Track.Title,
                TrackDescription = at.Track.Description,
                EventId = eventId,
                EventName = fullAssignEvent.Event?.Name ?? ""
            }).ToList();
    }

    public async Task<GetTrackSubmissionsResponse> GetTrackSubmissions(Guid trackId, Guid? roundId, bool? isGraded, int pageIndex, int pageSize)
    {
        var currentUserId = GetCurrentUserId();
        PaginationHelper.Validate(pageIndex, pageSize);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null || track.IsDisable)
            throw new NotFoundException("Track Not Found");

        var assignTrack = await ValidateJudgeTrackAssignment(currentUserId, track.EventId, trackId);

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            track.EventId, roundId, trackId, null, null, null,
            pageIndex, pageSize);

        var submissions = items.Select(rd =>
        {
            var lastSubmission = SubmissionHelper.GetLastSubmission(rd);
            var myScore = lastSubmission?.Scores
                .FirstOrDefault(s => s.AssignTrackId == assignTrack.Id);
            return new TrackSubmissionItem
            {
                SubmissionId = lastSubmission?.Id,
                RoundDetailId = rd.Id,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                Url = lastSubmission?.Url,
                Description = lastSubmission?.Description,
                Status = lastSubmission?.Status?.ToString(),
                SubmittedAt = lastSubmission?.SubmittedAt,
                GradingStatus = myScore != null ? "Graded" : "Pending",
                ScoreId = myScore?.Id,
                TotalScore = myScore?.TotalScore
            };
        }).ToList();

        // Filter by isGraded
        if (isGraded.HasValue)
        {
            submissions = submissions
                .Where(s => isGraded.Value
                    ? s.GradingStatus == "Graded"
                    : s.GradingStatus == "Pending")
                .ToList();
        }

        return new GetTrackSubmissionsResponse
        {
            Items = submissions,
            TotalCount = isGraded.HasValue ? submissions.Count : totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetTrackSubmissionsResponse> GetPendingSubmissions(Guid eventId, Guid? trackId, Guid? roundId, int pageIndex, int pageSize)
    {
        var currentUserId = GetCurrentUserId();
        PaginationHelper.Validate(pageIndex, pageSize);

        await ValidateJudgeEventAssignment(currentUserId, eventId);

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            eventId, roundId, trackId, null, null, null,
            pageIndex, pageSize);

        var result = new List<TrackSubmissionItem>();
        foreach (var rd in items)
        {
            var lastSubmission = SubmissionHelper.GetLastSubmission(rd);
            if (lastSubmission == null) continue;

            // Check if current judge has already graded this submission
            var hasScore = lastSubmission.Scores.Any(s =>
            {
                var track = s.AssignTrack;
                return track != null
                    && track.AssignEvent.UserId == currentUserId
                    && track.AssignEvent.EventRole != null
                    && track.AssignEvent.EventRole.Name == EventRoleEnum.Judge;
            });

            if (hasScore) continue;

            result.Add(new TrackSubmissionItem
            {
                SubmissionId = lastSubmission.Id,
                RoundDetailId = rd.Id,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                Url = lastSubmission.Url,
                Description = lastSubmission.Description,
                Status = lastSubmission.Status?.ToString(),
                SubmittedAt = lastSubmission.SubmittedAt,
                GradingStatus = "Pending"
            });
        }

        return new GetTrackSubmissionsResponse
        {
            Items = result,
            TotalCount = result.Count,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<SubmissionCriteriaResponse> GetSubmissionCriteria(Guid submissionId)
    {
        var currentUserId = GetCurrentUserId();

        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException("Submission Not Found");

        var registerTeam = submission.RoundDetail?.RegisterTeam;
        if (registerTeam == null)
            throw new NotFoundException("Register Team Not Found");

        // Try to get track from register team, or from submission's scores' assign tracks
        Guid eventId = registerTeam.EventId;
        Guid? trackId = registerTeam.TrackId;
        if (trackId == null)
        {
            trackId = submission.Scores
                .Select(s => s.AssignTrack?.TrackId)
                .FirstOrDefault(t => t.HasValue);
        }
        if (!trackId.HasValue)
            throw new BadRequestException("Register Team Has No Track Assigned");

        await ValidateJudgeTrackAssignment(currentUserId, eventId, trackId.Value);

        var roundDetail = submission.RoundDetail;
        var template = await _criteriaTemplateRepository.GetActiveByRoundIdAsync(roundDetail!.RoundId);

        return new SubmissionCriteriaResponse
        {
            SubmissionId = submissionId,
            RoundId = roundDetail.RoundId,
            TemplateId = template?.Id,
            TemplateTitle = template?.Title,
            CriteriaItems = template?.CriteriaItems
                .Where(ci => !ci.IsDisable)
                .Select(ci => new CriteriaItemResponse
                {
                    Id = ci.Id,
                    Name = ci.Name,
                    Description = ci.Description,
                    MaxScore = ci.Score
                }).ToList() ?? []
        };
    }

    public async Task<JudgeSubmissionScoreResponse?> GetMySubmissionScore(Guid submissionId)
    {
        var currentUserId = GetCurrentUserId();

        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException("Submission Not Found");

        var registerTeam = submission.RoundDetail?.RegisterTeam;
        if (registerTeam == null)
            throw new NotFoundException("Register Team Not Found");

        // Try to get track from register team, or from submission's scores' assign tracks
        Guid eventId = registerTeam.EventId;
        Guid? trackId = registerTeam.TrackId;
        if (trackId == null)
        {
            trackId = submission.Scores
                .Select(s => s.AssignTrack?.TrackId)
                .FirstOrDefault(t => t.HasValue);
        }
        if (!trackId.HasValue)
            throw new BadRequestException("Register Team Has No Track Assigned");

        var assignTrack = await _assignEventRepository.GetGraderAssignTrackAsync(
            currentUserId, eventId, trackId.Value);
        if (assignTrack == null)
            throw new ForbiddenException("You Are Not Assigned as Judge for This Track");

        // Find my score for this submission
        var myScore = submission.Scores
            .FirstOrDefault(s => s.AssignTrackId == assignTrack.Id);

        if (myScore == null)
            return null;

        return MapToJudgeSubmissionScoreResponse(myScore);
    }

    public async Task<JudgeSubmissionScoreResponse> SubmitScore(Guid submissionId, SubmitScoreRequest request)
    {
        var currentUserId = GetCurrentUserId();

        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException("Submission Not Found");

        var registerTeam = submission.RoundDetail?.RegisterTeam;
        if (registerTeam == null)
            throw new NotFoundException("Register Team Not Found");

        Guid? trackId = registerTeam.TrackId;
        if (trackId == null)
        {
            trackId = submission.Scores
                .Select(s => s.AssignTrack?.TrackId)
                .FirstOrDefault(t => t.HasValue);
        }
        if (!trackId.HasValue)
            throw new BadRequestException("Register Team Has No Track Assigned");

        var assignTrack = await ValidateJudgeTrackAssignment(currentUserId, registerTeam.EventId, trackId.Value);

        // Get active criteria template
        var template = await _criteriaTemplateRepository.GetActiveByRoundIdAsync(submission.RoundDetail!.RoundId);
        if (template == null)
            throw new BadRequestException("No Active Criteria Template Found for This Round");

        var submittedItems = request.Scores
            .Where(s => s.CriteriaItemId != Guid.Empty)
            .ToDictionary(
                s => s.CriteriaItemId,
                s => ((decimal?)(s.Score), s.Comment));

        var score = ScoreSubmissionHelper.CreateScore(
            submissionId,
            assignTrack.Id,
            isMock: false,
            template.CriteriaItems.Where(ci => !ci.IsDisable).ToList(),
            submittedItems);

        // Override TotalScore with the one from request
        score.TotalScore = request.TotalScore;
        score.IsRetake = false;

        await _scoreRepository.AddAsync(score);
        await _unitOfWork.SaveChangesAsync();

        return MapToJudgeSubmissionScoreResponse(score);
    }

    public async Task<JudgeSubmissionScoreResponse> UpdateScore(Guid scoreId, SubmitScoreRequest request)
    {
        var currentUserId = GetCurrentUserId();

        var score = await _scoreRepository.GetByIdAsync(scoreId);
        if (score == null)
            throw new NotFoundException("Score Not Found");

        // Verify this score belongs to the current judge
        if (score.AssignTrack?.AssignEvent?.UserId != currentUserId)
            throw new ForbiddenException("You Are Not Authorized to Update This Score");

        // Update each score item
        decimal total = 0m;
        foreach (var itemInput in request.Scores)
        {
            var scoreItem = score.ScoreItems
                .FirstOrDefault(si => si.CriteriaItemId == itemInput.CriteriaItemId);
            if (scoreItem != null)
            {
                scoreItem.Score = itemInput.Score;
                scoreItem.Comment = itemInput.Comment;
                scoreItem.UpdatedAt = DateTimeOffset.UtcNow;
            }
            total += itemInput.Score;
        }

        // Use TotalScore from request (FE có thể tự tính hoặc override)
        score.TotalScore = request.TotalScore;
        score.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return MapToJudgeSubmissionScoreResponse(score);
    }

    public async Task<JudgeScoreItemResponse> UpdateScoreItem(Guid scoreId, Guid scoreItemId, UpdateScoreItemRequest request)
    {
        var currentUserId = GetCurrentUserId();

        var scoreItem = await _scoreRepository.GetScoreItemByIdAsync(scoreItemId);
        if (scoreItem == null)
            throw new NotFoundException("Score Item Not Found");

        if (scoreItem.ScoreEntity?.AssignTrack?.AssignEvent?.UserId != currentUserId)
            throw new ForbiddenException("You Are Not Authorized to Update This Score Item");

        if (request.Score.HasValue)
            scoreItem.Score = request.Score;
        if (request.Comment != null)
            scoreItem.Comment = request.Comment;

        scoreItem.UpdatedAt = DateTimeOffset.UtcNow;

        // Recalculate TotalScore of parent Score
        var score = scoreItem.ScoreEntity;
        if (score != null)
        {
            score.TotalScore = score.ScoreItems.Sum(si => si.Score ?? 0);
            score.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync();

        return new JudgeScoreItemResponse
        {
            CriteriaItemId = scoreItem.CriteriaItemId,
            CriteriaItemName = scoreItem.CriteriaItem?.Name ?? "",
            Score = scoreItem.Score,
            Comment = scoreItem.Comment
        };
    }

    public async Task<string> FinalizeScore(Guid scoreId)
    {
        var currentUserId = GetCurrentUserId();

        var score = await _scoreRepository.GetByIdAsync(scoreId);
        if (score == null)
            throw new NotFoundException("Score Not Found");

        if (score.AssignTrack?.AssignEvent?.UserId != currentUserId)
            throw new ForbiddenException("You Are Not Authorized to Finalize This Score");

        score.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();

        return "Score Finalized Successfully";
    }

    public async Task<GetMyScoresResponse> GetMyScores(Guid eventId, Guid? trackId, bool? isGraded, int pageIndex, int pageSize)
    {
        var currentUserId = GetCurrentUserId();
        PaginationHelper.Validate(pageIndex, pageSize);

        await ValidateJudgeEventAssignment(currentUserId, eventId);

        // Lấy danh sách scores của judge trong event này
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(eventId, currentUserId);
        if (assignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");

        var myScores = new List<JudgeMyScoreItem>();
        foreach (var at in assignEvent.AssignTracks.Where(a => !a.IsDisable))
        {
            if (trackId.HasValue && at.TrackId != trackId.Value) continue;

            var scores = await _scoreRepository.GetByAssignTrackIdAsync(at.Id);
            foreach (var s in scores)
            {
                var sub = s.Submission;
                if (sub?.RoundDetail?.RegisterTeam == null) continue;

                var rt = sub.RoundDetail.RegisterTeam;
                myScores.Add(new JudgeMyScoreItem
                {
                    ScoreId = s.Id,
                    SubmissionId = s.SubmissionId,
                    TrackId = rt.TrackId!.Value,
                    TrackTitle = rt.Track?.Title ?? "",
                    TeamId = rt.TeamId,
                    TeamName = rt.Team.Name,
                    TotalScore = s.TotalScore,
                    IsRetake = s.IsRetake,
                    IsMock = s.IsMock,
                    SubmittedAt = sub.SubmittedAt,
                    UpdatedAt = s.UpdatedAt
                });
            }
        }

        // Filter isGraded — tất cả scores trong list này đều đã graded (có score)
        // isGraded=false: submissions chưa chấm — khác logic, dùng GetPendingSubmissions
        if (isGraded.HasValue && !isGraded.Value)
        {
            myScores = new List<JudgeMyScoreItem>();
        }

        var totalCount = myScores.Count;
        var paged = myScores
            .OrderByDescending(s => s.UpdatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new GetMyScoresResponse
        {
            Items = paged,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    private static JudgeSubmissionScoreResponse MapToJudgeSubmissionScoreResponse(Scores score)
    {
        return new JudgeSubmissionScoreResponse
        {
            ScoreId = score.Id,
            SubmissionId = score.SubmissionId,
            AssignTrackId = score.AssignTrackId,
            RetakeFromScoreId = score.RetakeFromScoreId,
            TotalScore = score.TotalScore,
            IsRetake = score.IsRetake,
            IsMock = score.IsMock,
            ScoreItems = score.ScoreItems.Select(si => new JudgeScoreItemResponse
            {
                CriteriaItemId = si.CriteriaItemId,
                CriteriaItemName = si.CriteriaItem?.Name ?? "",
                Score = si.Score,
                Comment = si.Comment
            }).ToList()
        };
    }
}
