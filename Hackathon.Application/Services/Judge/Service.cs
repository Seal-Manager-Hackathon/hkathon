using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Score;
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
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IRoundRepository _roundRepository;
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
        IRegisterTeamRepository registerTeamRepository,
        IRoundRepository roundRepository,
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
        _registerTeamRepository = registerTeamRepository;
        _roundRepository = roundRepository;
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
            var rt = rd.RegisterTeam;
            return new TrackSubmissionItem
            {
                RegisterTeamId = rt.Id,
                TeamId = rt.TeamId,
                TeamName = rt.Team.Name,
                EventId = rt.EventId,
                EventName = rt.Event?.Name ?? "",
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TrackId = rt.TrackId,
                TrackTitle = rt.Track?.Title,
                TopicId = rt.TopicId,
                TopicTitle = rt.Topic?.Title,
                SubmittedBy = GetTeamLeader(rt.Team.TeamDetails),
                LastSubmission = lastSubmission != null
                    ? new LastSubmissionInfo
                    {
                        Id = lastSubmission.Id,
                        SubmittedAt = lastSubmission.SubmittedAt,
                        Url = lastSubmission.Url,
                        Description = lastSubmission.Description,
                        Status = lastSubmission.Status?.ToString()
                    }
                    : null,
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

    public async Task<GetTrackSubmissionsResponse> GetMyScope(Guid eventId, Guid? trackId, Guid? roundId, string? status, int pageIndex, int pageSize)
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
            var myScore = lastSubmission.Scores.FirstOrDefault(s =>
            {
                var track = s.AssignTrack;
                return track != null
                    && track.AssignEvent.UserId == currentUserId
                    && track.AssignEvent.EventRole != null
                    && track.AssignEvent.EventRole.Name == EventRoleEnum.Judge;
            });

            var gradingStatus = myScore != null ? "Graded" : "Pending";

            // Filter by status
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (!status.Equals(gradingStatus, StringComparison.OrdinalIgnoreCase))
                    continue;
            }

            var rt = rd.RegisterTeam;
            result.Add(new TrackSubmissionItem
            {
                RegisterTeamId = rt.Id,
                TeamId = rt.TeamId,
                TeamName = rt.Team.Name,
                EventId = rt.EventId,
                EventName = rt.Event?.Name ?? "",
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TrackId = rt.TrackId,
                TrackTitle = rt.Track?.Title,
                TopicId = rt.TopicId,
                TopicTitle = rt.Topic?.Title,
                SubmittedBy = GetTeamLeader(rt.Team.TeamDetails),
                LastSubmission = new LastSubmissionInfo
                {
                    Id = lastSubmission.Id,
                    SubmittedAt = lastSubmission.SubmittedAt,
                    Url = lastSubmission.Url,
                    Description = lastSubmission.Description,
                    Status = lastSubmission.Status?.ToString()
                },
                GradingStatus = gradingStatus,
                ScoreId = myScore?.Id,
                TotalScore = myScore?.TotalScore
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

        if (template == null)
        {
            return new SubmissionCriteriaResponse
            {
                SubmissionId = submissionId,
                Id = Guid.Empty,
                RoundId = roundDetail.RoundId,
                Title = "",
                Items = []
            };
        }

        return new SubmissionCriteriaResponse
        {
            SubmissionId = submissionId,
            Id = template.Id,
            RoundId = template.RoundId,
            Title = template.Title,
            Description = template.Description,
            IsDisable = template.IsDisable,
            IsActive = template.IsActive,
            Items = template.CriteriaItems
                .Where(ci => !ci.IsDisable)
                .Select(ci => new CriteriaItemDetail
                {
                    Id = ci.Id,
                    CriteriaTemplateId = ci.CriteriaTemplateId,
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

    // GetMySubmissionScore removed — use GetEventSubmissions or GetMyScores instead

    public async Task<JudgeSubmissionScoreResponse> SubmitScore(Guid submissionId, SubmitScoreRequest request)
    {
        var currentUserId = GetCurrentUserId();

        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException("Submission Not Found");

        var registerTeam = submission.RoundDetail?.RegisterTeam;
        if (registerTeam == null)
            throw new NotFoundException("Register Team Not Found");

        var round = submission.RoundDetail?.Round;
        if (round == null)
            throw new NotFoundException("Round Not Found");

        // Validate round đã qua EndSubmission chưa
        if (round.EndSubmission.HasValue && round.EndSubmission.Value > DateTimeOffset.UtcNow)
            throw new BadRequestException("Submission Period Has Not Ended Yet. Cannot Grade Before EndSubmission.");

        // Validate event chưa kết thúc
        var ev = await _eventRepository.GetByIdAsync(registerTeam.EventId);
        if (ev != null && ev.EndTime.HasValue && ev.EndTime.Value <= DateTimeOffset.UtcNow)
            throw new BadRequestException("Event Has Ended. Cannot Grade.");

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

        var activeItems = template.CriteriaItems.Where(ci => !ci.IsDisable).ToList();

        // Validate: judge phải chấm đủ tất cả tiêu chí
        var submittedIds = request.Scores
            .Where(s => s.CriteriaItemId != Guid.Empty)
            .Select(s => s.CriteriaItemId)
            .ToHashSet();

        var missingItems = activeItems
            .Where(ci => !submittedIds.Contains(ci.Id))
            .ToList();

        if (missingItems.Count > 0)
        {
            var missingNames = string.Join(", ", missingItems.Select(ci => $"\"{ci.Name}\""));
            throw new BadRequestException($"Missing Criteria Items: {missingNames}");
        }

        var submittedItems = request.Scores
            .Where(s => s.CriteriaItemId != Guid.Empty)
            .ToDictionary(
                s => s.CriteriaItemId,
                s => ((decimal?)(s.Score), s.Comment));

        var score = ScoreSubmissionHelper.CreateScore(
            submissionId,
            assignTrack.Id,
            isMock: false,
            activeItems,
            submittedItems);

        score.IsRetake = false;

        await _scoreRepository.AddAsync(score);
        await _unitOfWork.SaveChangesAsync();

        return MapToJudgeSubmissionScoreResponse(score);
    }

    public async Task<UpdateScoreResponse> UpdateScore(Guid scoreId, SubmitScoreRequest request, int pageIndex = 1, int pageSize = 10)
    {
        var currentUserId = GetCurrentUserId();
        PaginationHelper.Validate(pageIndex, pageSize);

        var score = await _scoreRepository.GetByIdAsync(scoreId);
        if (score == null)
            throw new NotFoundException("Score Not Found");

        // Verify this score belongs to the current judge
        if (score.AssignTrack?.AssignEvent?.UserId != currentUserId)
            throw new ForbiddenException("You Are Not Authorized to Update This Score");

        // Validate event chưa kết thúc
        var registerTeam = score.Submission?.RoundDetail?.RegisterTeam;
        if (registerTeam != null)
        {
            var ev = await _eventRepository.GetByIdAsync(registerTeam.EventId);
            if (ev != null && ev.EndTime.HasValue && ev.EndTime.Value <= DateTimeOffset.UtcNow)
                throw new BadRequestException("Event Has Ended. Cannot Update Score.");
        }

        // Track which criteria items were updated
        var updatedIds = new HashSet<Guid>();

        // Update each score item (chỉ sửa items gửi lên, giữ nguyên items ko gửi)
        foreach (var itemInput in request.Scores)
        {
            var scoreItem = score.ScoreItems
                .FirstOrDefault(si => si.CriteriaItemId == itemInput.CriteriaItemId);
            if (scoreItem != null)
            {
                scoreItem.Score = itemInput.Score;
                scoreItem.Comment = itemInput.Comment;
                scoreItem.UpdatedAt = DateTimeOffset.UtcNow;
                updatedIds.Add(scoreItem.Id);
            }
        }

        // Auto-calculate TotalScore as SUM of all ScoreItems
        score.TotalScore = score.ScoreItems.Sum(si => si.Score ?? 0);
        score.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        // Trả về paginated list of score items với flag isUpdated
        var allItems = score.ScoreItems
            .OrderBy(si => si.CreatedAt)
            .ToList();

        var totalCount = allItems.Count;
        var paged = allItems
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new UpdateScoreResponse
        {
            ScoreId = scoreId,
            Items = paged.Select(si => new UpdatedScoreItemResponse
            {
                ScoreItemId = si.Id,
                ScoreId = si.ScoreId,
                SubmissionId = score.SubmissionId,
                CriteriaItemId = si.CriteriaItemId,
                CriteriaItemName = si.CriteriaItem?.Name ?? "",
                Score = si.Score,
                Comment = si.Comment,
                GradedByUserId = si.AssignTrack?.AssignEvent?.UserId,
                IsUpdated = updatedIds.Contains(si.Id)
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<UpdatedScoreItemResponse> UpdateScoreItem(Guid scoreItemId, UpdateScoreItemRequest request)
    {
        var currentUserId = GetCurrentUserId();

        var scoreItem = await _scoreRepository.GetScoreItemByIdAsync(scoreItemId);
        if (scoreItem == null)
            throw new NotFoundException("Score Item Not Found");

        if (scoreItem.ScoreEntity?.AssignTrack?.AssignEvent?.UserId != currentUserId)
            throw new ForbiddenException("You Are Not Authorized to Update This Score Item");

        // Validate event chưa kết thúc
        var registerTeam = scoreItem.ScoreEntity?.Submission?.RoundDetail?.RegisterTeam;
        if (registerTeam != null)
        {
            var ev = await _eventRepository.GetByIdAsync(registerTeam.EventId);
            if (ev != null && ev.EndTime.HasValue && ev.EndTime.Value <= DateTimeOffset.UtcNow)
                throw new BadRequestException("Event Has Ended. Cannot Update Score.");
        }

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

        var subId = scoreItem.ScoreEntity?.SubmissionId;

        return new UpdatedScoreItemResponse
        {
            ScoreItemId = scoreItem.Id,
            ScoreId = scoreItem.ScoreId,
            SubmissionId = subId ?? Guid.Empty,
            CriteriaItemId = scoreItem.CriteriaItemId,
            CriteriaItemName = scoreItem.CriteriaItem?.Name ?? "",
            Score = scoreItem.Score,
            Comment = scoreItem.Comment,
            GradedByUserId = scoreItem.AssignTrack?.AssignEvent?.UserId,
            IsUpdated = true
        };
    }

    public async Task<GetMyScoresResponse> GetMyScores(Guid eventId, Guid? roundId, Guid? trackId, bool? isGraded, int pageIndex, int pageSize)
    {
        var currentUserId = GetCurrentUserId();
        PaginationHelper.Validate(pageIndex, pageSize);

        await ValidateJudgeEventAssignment(currentUserId, eventId);

        // Lấy assign tracks của judge trong event này
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(eventId, currentUserId);
        if (assignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");

        var myAssignTrackIds = assignEvent.AssignTracks
            .Where(at => !at.IsDisable)
            .Select(at => at.Id)
            .ToHashSet();

        if (myAssignTrackIds.Count == 0)
        {
            return new GetMyScoresResponse
            {
                Items = new List<JudgeMyScoreItem>(),
                TotalCount = 0,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        // Lấy all submissions trong event, filter theo track/round
        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            eventId, roundId, trackId, null, null, null,
            1, int.MaxValue);

        var result = new List<JudgeMyScoreItem>();
        foreach (var rd in items)
        {
            var lastSubmission = SubmissionHelper.GetLastSubmission(rd);
            if (lastSubmission == null) continue;

            // Kiểm tra submission này có thuộc track judge được phân công ko
            Guid? submissionTrackId = rd.RegisterTeam.TrackId;
            bool belongsToMyTrack = submissionTrackId.HasValue
                && assignEvent.AssignTracks.Any(at =>
                    at.TrackId == submissionTrackId.Value && !at.IsDisable);

            if (!belongsToMyTrack) continue;

            // Tìm score của chính judge này (nếu đã chấm)
            var myScore = lastSubmission.Scores
                .FirstOrDefault(s => myAssignTrackIds.Contains(s.AssignTrackId));

            result.Add(new JudgeMyScoreItem
            {
                RegisterTeamId = rd.RegisterTeamId,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                TrackId = rd.RegisterTeam.TrackId,
                TrackTitle = rd.RegisterTeam.Track?.Title,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                SubmissionId = lastSubmission.Id,
                Url = lastSubmission.Url,
                SubmittedAt = lastSubmission.SubmittedAt,
                GradingStatus = myScore != null ? "Graded" : "Pending",
                ScoreId = myScore?.Id,
                TotalScore = myScore?.TotalScore
            });
        }

        // Filter isGraded
        if (isGraded.HasValue)
        {
            result = result
                .Where(s => isGraded.Value
                    ? s.GradingStatus == "Graded"
                    : s.GradingStatus == "Pending")
                .ToList();
        }

        var paged = result
            .OrderByDescending(s => s.SubmittedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new GetMyScoresResponse
        {
            Items = paged,
            TotalCount = result.Count,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetScoreItemsResponse> GetScoreItems(Guid scoreId, int pageIndex, int pageSize)
    {
        var currentUserId = GetCurrentUserId();
        PaginationHelper.Validate(pageIndex, pageSize);

        var score = await _scoreRepository.GetByIdAsync(scoreId);
        if (score == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Verify judge owns this score
        if (score.AssignTrack?.AssignEvent?.UserId != currentUserId)
            throw new ForbiddenException("You Are Not Authorized to View This Score");

        var (items, totalCount) = await _scoreRepository.GetScoreItemsByScoreIdAsync(scoreId, pageIndex, pageSize);

        return new GetScoreItemsResponse
        {
            ScoreId = scoreId,
            Items = items.Select(si =>
            {
                var rt = si.ScoreEntity?.Submission?.RoundDetail?.RegisterTeam;
                return new ScoreItemDetail
                {
                    ScoreItemId = si.Id,
                    ScoreId = si.ScoreId,
                    CriteriaItemId = si.CriteriaItemId,
                    AssignTrackId = si.AssignTrackId,
                    AssignEventId = si.AssignTrack?.AssignEventId ?? Guid.Empty,
                    CriteriaName = si.CriteriaItem?.Name ?? "",
                    Score = si.Score,
                    Comment = si.Comment,
                    GradedBy = si.AssignTrack?.AssignEvent?.User != null
                        ? new GraderInfo
                        {
                            UserId = si.AssignTrack.AssignEvent.User.Id,
                            Email = si.AssignTrack.AssignEvent.User.Email,
                            FirstName = si.AssignTrack.AssignEvent.User.FirstName,
                            LastName = si.AssignTrack.AssignEvent.User.LastName
                        }
                        : null,
                    TrackTitle = rt?.Track?.Title,
                    TrackId = rt?.TrackId,
                    TopicId = rt?.TopicId,
                    TopicTitle = rt?.Topic?.Title,
                    CreatedAt = si.CreatedAt,
                    UpdatedAt = si.UpdatedAt
                };
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<ScoreItemDetail> GetScoreItemDetail(Guid scoreItemId)
    {
        var currentUserId = GetCurrentUserId();

        var scoreItem = await _scoreRepository.GetScoreItemByIdAsync(scoreItemId);
        if (scoreItem == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Verify judge owns this score item
        if (scoreItem.AssignTrack?.AssignEvent?.UserId != currentUserId)
            throw new ForbiddenException("You Are Not Authorized to View This Score Item");

        var rt = scoreItem.ScoreEntity?.Submission?.RoundDetail?.RegisterTeam;

        return new ScoreItemDetail
        {
            ScoreItemId = scoreItem.Id,
            ScoreId = scoreItem.ScoreId,
            CriteriaItemId = scoreItem.CriteriaItemId,
            AssignTrackId = scoreItem.AssignTrackId,
            AssignEventId = scoreItem.AssignTrack?.AssignEventId ?? Guid.Empty,
            CriteriaName = scoreItem.CriteriaItem?.Name ?? "",
            Score = scoreItem.Score,
            Comment = scoreItem.Comment,
            GradedBy = scoreItem.AssignTrack?.AssignEvent?.User != null
                ? new GraderInfo
                {
                    UserId = scoreItem.AssignTrack.AssignEvent.User.Id,
                    Email = scoreItem.AssignTrack.AssignEvent.User.Email,
                    FirstName = scoreItem.AssignTrack.AssignEvent.User.FirstName,
                    LastName = scoreItem.AssignTrack.AssignEvent.User.LastName
                }
                : null,
            TrackTitle = rt?.Track?.Title,
            TrackId = rt?.TrackId,
            TopicId = rt?.TopicId,
            TopicTitle = rt?.Topic?.Title,
            CreatedAt = scoreItem.CreatedAt,
            UpdatedAt = scoreItem.UpdatedAt
        };
    }

    public async Task<GetRegisterTeamSubmissionsResponse> GetRegisterTeamSubmissions(Guid registerTeamId)
    {
        var currentUserId = GetCurrentUserId();

        var registerTeam = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (registerTeam == null)
            throw new NotFoundException("Register Team Not Found");

        // Validate judge is assigned to this event
        await ValidateJudgeEventAssignment(currentUserId, registerTeam.EventId);

        // Get all rounds with submissions for this register team
        var (items, _) = await _submissionRepository.GetSubmissionsAsync(
            registerTeam.EventId, null, null, null, registerTeamId, null,
            1, int.MaxValue);

        // Find the absolute latest submission across all rounds
        var allSubmissions = items
            .SelectMany(rd => rd.Submissions
                .Select(s => new { RoundDetail = rd, Submission = s }))
            .OrderByDescending(x => x.Submission.SubmittedAt)
            .ToList();

        var latest = allSubmissions.FirstOrDefault();
        if (latest == null)
        {
            // No submissions at all — return basic info with no lastSubmission
            return new GetRegisterTeamSubmissionsResponse
            {
                RegisterTeamId = registerTeam.Id,
                TeamId = registerTeam.TeamId,
                TeamName = registerTeam.Team.Name,
                EventId = registerTeam.EventId,
                EventName = registerTeam.Event?.Name ?? "",
                TrackId = registerTeam.TrackId,
                TrackTitle = registerTeam.Track?.Title,
                TopicId = registerTeam.TopicId,
                TopicTitle = registerTeam.Topic?.Title,
                RoundId = Guid.Empty,
                RoundName = "",
                SubmittedBy = GetTeamLeader(registerTeam.Team.TeamDetails),
                LastSubmission = null,
                GradingStatus = "Pending",
                ScoreId = null,
                TotalScore = null
            };
        }

        // Find the current judge's score for this submission
        var myScore = latest.Submission.Scores.FirstOrDefault(s =>
        {
            var track = s.AssignTrack;
            return track != null
                && track.AssignEvent.UserId == currentUserId
                && track.AssignEvent.EventRole != null
                && track.AssignEvent.EventRole.Name == EventRoleEnum.Judge;
        });

        return new GetRegisterTeamSubmissionsResponse
        {
            RegisterTeamId = registerTeam.Id,
            TeamId = registerTeam.TeamId,
            TeamName = registerTeam.Team.Name,
            EventId = registerTeam.EventId,
            EventName = registerTeam.Event?.Name ?? "",
            RoundId = latest.RoundDetail.RoundId,
            RoundName = latest.RoundDetail.Round.Name,
            TrackId = registerTeam.TrackId,
            TrackTitle = registerTeam.Track?.Title,
            TopicId = registerTeam.TopicId,
            TopicTitle = registerTeam.Topic?.Title,
            SubmittedBy = GetTeamLeader(registerTeam.Team.TeamDetails),
            LastSubmission = new LastSubmissionInfo
            {
                Id = latest.Submission.Id,
                SubmittedAt = latest.Submission.SubmittedAt,
                Url = latest.Submission.Url,
                Description = latest.Submission.Description,
                Status = latest.Submission.Status?.ToString()
            },
            GradingStatus = myScore != null ? "Graded" : "Pending",
            ScoreId = myScore?.Id,
            TotalScore = myScore?.TotalScore
        };
    }

    public async Task<GetTrackSubmissionsResponse> GetSubmissionsByRound(Guid roundId, Guid? trackId, int pageIndex, int pageSize)
    {
        var currentUserId = GetCurrentUserId();
        PaginationHelper.Validate(pageIndex, pageSize);

        var round = await _roundRepository.GetDetailByIdAsync(roundId);
        if (round == null || round.IsDisable)
            throw new NotFoundException("Round Not Found");

        // Validate judge is assigned to this event
        await ValidateJudgeEventAssignment(currentUserId, round.EventId);

        // Get judge's assigned track IDs for this event
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(round.EventId, currentUserId);
        if (assignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");

        var myTrackIds = assignEvent.AssignTracks
            .Where(at => !at.IsDisable && !at.Track.IsDisable)
            .Select(at => at.TrackId)
            .ToList();

        if (myTrackIds.Count == 0)
        {
            return new GetTrackSubmissionsResponse
            {
                Items = new List<TrackSubmissionItem>(),
                TotalCount = 0,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        // If trackId provided, validate judge is assigned to that specific track
        if (trackId.HasValue)
        {
            if (!myTrackIds.Contains(trackId.Value))
                throw new ForbiddenException("You Are Not Assigned as Judge for This Track");

            // Filter only to this one track
            var (singleItems, singleTotal) = await _submissionRepository.GetRoundSubmissionsAsync(
                roundId, new List<Guid> { trackId.Value }, pageIndex, pageSize);

            var submissions = MapToTrackSubmissions(singleItems, assignEvent, round.EventId);
            return new GetTrackSubmissionsResponse
            {
                Items = submissions,
                TotalCount = singleTotal,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        // No trackId: return submissions from ALL assigned tracks
        var (items, totalCount) = await _submissionRepository.GetRoundSubmissionsAsync(
            roundId, myTrackIds, pageIndex, pageSize);

        var result = MapToTrackSubmissions(items, assignEvent, round.EventId);
        return new GetTrackSubmissionsResponse
        {
            Items = result,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<JudgeSubmissionDetailResponse> GetSubmissionDetail(Guid submissionId)
    {
        var currentUserId = GetCurrentUserId();

        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Validate judge is assigned to the event
        var eventId = submission.RoundDetail?.RegisterTeam?.EventId;
        if (eventId.HasValue)
            await ValidateJudgeEventAssignment(currentUserId, eventId.Value);

        var validScores = submission.Scores.Where(s => s.TotalScore.HasValue).ToList();
        var totalScore = validScores.Count > 0
            ? Math.Round(validScores.Sum(s => s.TotalScore!.Value), 2)
            : (decimal?)null;

        return new JudgeSubmissionDetailResponse
        {
            Id = submission.Id,
            RoundDetailId = submission.RoundDetailId,
            RoundId = submission.RoundDetail.RoundId,
            RoundName = submission.RoundDetail.Round.Name,
            RegisterTeamId = submission.RoundDetail.RegisterTeamId,
            TeamId = submission.RoundDetail.RegisterTeam.TeamId,
            TeamName = submission.RoundDetail.RegisterTeam.Team.Name,
            TrackId = submission.RoundDetail.RegisterTeam.TrackId,
            TrackTitle = submission.RoundDetail.RegisterTeam.Track?.Title,
            TopicId = submission.RoundDetail.RegisterTeam.TopicId,
            TopicTitle = submission.RoundDetail.RegisterTeam.Topic?.Title,
            Url = submission.Url,
            Description = submission.Description,
            Status = submission.Status?.ToString(),
            SubmittedAt = submission.SubmittedAt,
            IsRegrade = submission.IsRegrade,
            SubmittedBy = submission.RoundDetail.RegisterTeam.Team.TeamDetails
                .Where(td => td.IsLeader)
                .Select(td => new JudgeSubmittedByUser
                {
                    UserId = td.UserId,
                    Email = td.User.Email,
                    FirstName = td.User.FirstName,
                    LastName = td.User.LastName
                })
                .FirstOrDefault(),
            TotalScore = totalScore,
            JudgeCount = validScores.Count,
            CreatedAt = submission.CreatedAt,
            UpdatedAt = submission.UpdatedAt
        };
    }

    private List<TrackSubmissionItem> MapToTrackSubmissions(List<RoundDetails> items, AssignEvents assignEvent, Guid eventId)
    {
        return items.Select(rd =>
        {
            var lastSubmission = SubmissionHelper.GetLastSubmission(rd);
            var myAssignTrackIds = assignEvent.AssignTracks
                .Where(at => !at.IsDisable)
                .Select(at => at.Id)
                .ToHashSet();
            var myScore = lastSubmission?.Scores
                .FirstOrDefault(s => myAssignTrackIds.Contains(s.AssignTrackId));
            var rt = rd.RegisterTeam;
            return new TrackSubmissionItem
            {
                RegisterTeamId = rt.Id,
                TeamId = rt.TeamId,
                TeamName = rt.Team.Name,
                EventId = rt.EventId,
                EventName = rt.Event?.Name ?? "",
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TrackId = rt.TrackId,
                TrackTitle = rt.Track?.Title,
                TopicId = rt.TopicId,
                TopicTitle = rt.Topic?.Title,
                SubmittedBy = GetTeamLeader(rt.Team.TeamDetails),
                LastSubmission = lastSubmission != null
                    ? new LastSubmissionInfo
                    {
                        Id = lastSubmission.Id,
                        SubmittedAt = lastSubmission.SubmittedAt,
                        Url = lastSubmission.Url,
                        Description = lastSubmission.Description,
                        Status = lastSubmission.Status?.ToString()
                    }
                    : null,
                GradingStatus = myScore != null ? "Graded" : "Pending",
                ScoreId = myScore?.Id,
                TotalScore = myScore?.TotalScore
            };
        }).ToList();
    }

    private static SubmittedByInfo? GetTeamLeader(ICollection<TeamDetails>? teamDetails)
    {
        var leader = teamDetails?.FirstOrDefault(td => td.IsLeader);
        if (leader?.User == null) return null;
        return new SubmittedByInfo
        {
            UserId = leader.User.Id,
            Email = leader.User.Email,
            FirstName = leader.User.FirstName,
            LastName = leader.User.LastName
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
