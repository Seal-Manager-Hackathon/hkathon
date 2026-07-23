using Hackathon.Application.Common;
using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Helpers.Notification;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.RegisterTeam;

public class Service : IRegisterTeamService
{
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IRegisterTeamRepository registerTeamRepository,
        IEventRepository eventRepository,
        ITeamRepository teamRepository,
        IRoundRepository roundRepository,
        ITrackRepository trackRepository,
        ITopicRepository topicRepository,
        INotificationRepository notificationRepository,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _registerTeamRepository = registerTeamRepository;
        _eventRepository = eventRepository;
        _teamRepository = teamRepository;
        _roundRepository = roundRepository;
        _trackRepository = trackRepository;
        _topicRepository = topicRepository;
        _notificationRepository = notificationRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRegisterTeamsResponse> GetRegisterTeams(GetRegisterTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RegisterTeamStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected, Banned");
            status = parsed;
        }

        var (items, totalCount) = await _registerTeamRepository.SearchAsync(
            request.EventId, request.Keyword, status,
            request.IsBanned, request.IsDisable,
            request.FromDate, request.ToDate,
            request.RoundId, request.TrackId, request.TopicId,
            request.PageIndex, request.PageSize);

        return new GetRegisterTeamsResponse
        {
            RegisterTeams = items.Select(rt =>
            {
                var maxRound = rt.RoundDetails
                    .Where(rd => rd.Round != null && !rd.IsDisable)
                    .OrderByDescending(rd => rd.Round!.RoundNo)
                    .FirstOrDefault();

                return new RegisterTeamCard
                {
                    Id = rt.Id,
                    TeamId = rt.TeamId,
                    TeamName = rt.Team?.Name,
                    EventId = rt.EventId,
                    EventName = rt.Event?.Name,
                    TrackId = rt.TrackId,
                    TrackName = rt.Track?.Title,
                    TopicId = rt.TopicId,
                    TopicName = rt.Topic?.Title,
                    Description = rt.Description,
                    RejectionReason = rt.RejectionReason,
                    Status = rt.Status?.ToString(),
                    IsBanned = rt.IsBanned,
                    IsDisable = rt.IsDisable,
                    RoundId = maxRound?.RoundId,
                    RoundName = maxRound?.Round?.Name,
                    RoundNo = maxRound?.Round?.RoundNo,
                    CreatedAt = rt.CreatedAt,
                    UpdatedAt = rt.UpdatedAt
                };
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetRegisterTeamsWithScoresResponse> GetRegisterTeamsWithScores(GetRegisterTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RegisterTeamStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected, Banned");
            status = parsed;
        }

        // Lấy tất cả items để tính score rồi sort
        var (allItems, totalCount) = await _registerTeamRepository.SearchWithScoresAsync(
            request.EventId, request.Keyword, status,
            request.IsBanned, request.IsDisable,
            request.FromDate, request.ToDate,
            request.RoundId, request.TrackId, request.TopicId,
            1, int.MaxValue);

        var scored = allItems.Select(rt =>
        {
            var maxRound = rt.RoundDetails
                .Where(rd => rd.Round != null && !rd.IsDisable)
                .OrderByDescending(rd => rd.Round!.RoundNo)
                .FirstOrDefault();

            // scopeScore = AVG(Scores.TotalScore) của submission cuối trong round
            decimal? totalScopeScore = null;
            if (maxRound != null)
            {
                var lastSubmission = maxRound.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .FirstOrDefault();

                if (lastSubmission != null)
                {
                    var validScores = lastSubmission.Scores
                        .Where(s => s.TotalScore.HasValue)
                        .ToList();

                    if (validScores.Count > 0)
                        totalScopeScore = Math.Round(validScores.Average(s => s.TotalScore!.Value), 2);
                }
            }

            return new { Rt = rt, MaxRound = maxRound, TotalScopeScore = totalScopeScore };
        })
        .OrderByDescending(x => x.TotalScopeScore) // sort theo điểm cao nhất giảm dần
        .ToList();

        totalCount = scored.Count;

        // Phân trang sau khi sort
        var paged = scored
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new GetRegisterTeamsWithScoresResponse
        {
            RegisterTeams = paged.Select(x => new RegisterTeamWithScoreCard
            {
                Id = x.Rt.Id,
                TeamId = x.Rt.TeamId,
                TeamName = x.Rt.Team?.Name,
                EventId = x.Rt.EventId,
                EventName = x.Rt.Event?.Name,
                TrackId = x.Rt.TrackId,
                TrackName = x.Rt.Track?.Title,
                TopicId = x.Rt.TopicId,
                TopicName = x.Rt.Topic?.Title,
                Description = x.Rt.Description,
                RejectionReason = x.Rt.RejectionReason,
                Status = x.Rt.Status?.ToString(),
                IsBanned = x.Rt.IsBanned,
                IsDisable = x.Rt.IsDisable,
                RoundId = x.MaxRound?.RoundId,
                RoundName = x.MaxRound?.Round?.Name,
                RoundNo = x.MaxRound?.Round?.RoundNo,
                TotalScopeScore = x.TotalScopeScore,
                CreatedAt = x.Rt.CreatedAt,
                UpdatedAt = x.Rt.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        var members = await _teamRepository.GetTeamMembersAsync(rt.TeamId);

        var maxRound = rt.RoundDetails
            .Where(rd => rd.Round != null && !rd.IsDisable)
            .OrderByDescending(rd => rd.Round!.RoundNo)
            .FirstOrDefault();

        return new RegisterTeamDetailResponse
        {
            // RegisterTeam
            Id = rt.Id,
            Description = rt.Description,
            RejectionReason = rt.RejectionReason,
            Status = rt.Status?.ToString(),
            IsBanned = rt.IsBanned,
            IsDisable = rt.IsDisable,
            RoundId = maxRound?.RoundId,
            RoundName = maxRound?.Round?.Name,
            RoundNo = maxRound?.Round?.RoundNo,
            CreatedAt = rt.CreatedAt,
            UpdatedAt = rt.UpdatedAt,

            // Event
            EventId = rt.EventId,
            EventName = rt.Event?.Name,
            EventDescription = rt.Event?.Description,
            EventStartDate = rt.Event?.StartTime,
            EventEndDate = rt.Event?.EndTime,

            // Team
            TeamId = rt.TeamId,
            TeamName = rt.Team?.Name,
            TeamCanEdit = rt.Team?.CanEdit ?? false,
            TeamIsDisable = rt.Team?.IsDisable ?? false,
            TeamCreatedAt = rt.Team?.CreatedAt ?? default,

            // Track / Topic
            TrackId = rt.TrackId,
            TrackTitle = rt.Track?.Title,
            TopicId = rt.TopicId,
            TopicTitle = rt.Topic?.Title,

            // Members
            Members = members.Select(m => new RegisterTeamMemberItem
            {
                UserId = m.UserId,
                Email = m.User?.Email ?? "",
                FirstName = m.User?.FirstName ?? "",
                LastName = m.User?.LastName ?? "",
                AvatarUrl = m.User?.AvatarUrl,
                IsLeader = m.IsLeader,
                Status = m.Status?.ToString()
            }).ToList()
        };
    }

    public async Task UpdateRegisterTeam(UpdateRegisterTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(request.RegisterTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (request.Description != null)
            rt.Description = request.Description;
        if (request.RejectionReason != null)
            rt.RejectionReason = request.RejectionReason;
        if (request.Status != null)
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var status))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected, Banned");
            rt.Status = status;
        }
        if (request.IsBanned.HasValue)
            rt.IsBanned = request.IsBanned.Value;
        if (request.IsDisable.HasValue)
            rt.IsDisable = request.IsDisable.Value;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ApproveRegisterTeam(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.Status != RegisterTeamStatusEnum.Pending)
            throw new BadRequestException("Only Pending Register Team Can Be Approved");

        // [Commented] Check event time window — bỏ check để dễ test
        //var ev = await _eventRepository.GetByIdAsync(rt.EventId);
        //if (ev != null)
        //{
        //    if (ev.StartTime.HasValue && DateTimeOffset.UtcNow < ev.StartTime.Value)
        //        throw new BadRequestException("Cannot Approve Before Event Starts");
        //    if (ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
        //        throw new BadRequestException("Cannot Approve After Event Has Ended");
        //}

        // Check round No1 của event
        var firstRound = await _roundRepository.GetFirstRoundByEventIdAsync(rt.EventId);

        // [Commented] Phải approve trước khi round 1 bắt đầu — bỏ check để dễ test
        //if (firstRound != null && firstRound.StartTime.HasValue && DateTimeOffset.UtcNow >= firstRound.StartTime.Value)
        //    throw new BadRequestException("Cannot Approve After Round 1 Has Started");

        if (firstRound != null && firstRound.LimitTeam.HasValue)
        {
            var currentTeamCount = await _roundRepository.CountTeamsInRoundAsync(firstRound.Id);
            if (currentTeamCount >= firstRound.LimitTeam.Value)
                throw new BadRequestException("Round 1 Is Full. Cannot Approve More Teams");
        }

        // Check xung đột: thành viên trong team này đã được duyệt ở team khác trong cùng event chưa?
        var teamMembers = await _teamRepository.GetTeamMembersAsync(rt.TeamId);
        var memberUserIds = teamMembers.Where(td => !td.IsDisable).Select(td => td.UserId).ToList();
        if (memberUserIds.Count > 0)
        {
            var hasConflict = await _registerTeamRepository.HasAnyMemberApprovedInEventAsync(rt.EventId, memberUserIds);
            if (hasConflict)
                throw new BadRequestException("One Or More Team Members Are Already Approved In Another Team For This Event");
        }

        rt.Status = RegisterTeamStatusEnum.Approved;

        // Khóa team: không thể chỉnh sửa thành viên
        var team = await _teamRepository.GetByIdAsync(rt.TeamId);
        if (team != null)
        {
            team.CanEdit = false;
            await _teamRepository.UpdateAsync(team);
        }

        // Tự động thêm team vào round đầu tiên nếu có
        if (firstRound != null)
        {
            var roundDetail = new RoundDetails
            {
                Id = Guid.NewGuid(),
                RoundId = firstRound.Id,
                RegisterTeamId = registerTeamId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _roundRepository.AddRoundDetailAsync(roundDetail);
        }

        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team
        if (team != null && rt.Event != null)
        {
            var notification = NotificationHelper.Create(
                NotificationTargetTypeEnum.Team,
                "Registration Approved",
                string.Format(NotificationMessage.RegisterEvent.Approved, team.Name, rt.Event.Name),
                teamId: rt.TeamId);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(GetRegisterTeamsByTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RegisterTeamStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected, Banned");
            status = parsed;
        }

        var (items, totalCount) = await _registerTeamRepository.GetByTeamIdAsync(
            request.TeamId, status, request.IsDisable, request.PageIndex, request.PageSize);

        return new GetRegisterTeamsResponse
        {
            RegisterTeams = items.Select(rt =>
            {
                var maxRound = rt.RoundDetails
                    .Where(rd => rd.Round != null && !rd.IsDisable)
                    .OrderByDescending(rd => rd.Round!.RoundNo)
                    .FirstOrDefault();

                return new RegisterTeamCard
                {
                    Id = rt.Id,
                    TeamId = rt.TeamId,
                    TeamName = rt.Team?.Name,
                    EventId = rt.EventId,
                    EventName = rt.Event?.Name,
                    TrackId = rt.TrackId,
                    TrackName = rt.Track?.Title,
                    TopicId = rt.TopicId,
                    TopicName = rt.Topic?.Title,
                    Description = rt.Description,
                    RejectionReason = rt.RejectionReason,
                    Status = rt.Status?.ToString(),
                    IsBanned = rt.IsBanned,
                    IsDisable = rt.IsDisable,
                    RoundId = maxRound?.RoundId,
                    RoundName = maxRound?.Round?.Name,
                    RoundNo = maxRound?.Round?.RoundNo,
                    CreatedAt = rt.CreatedAt,
                    UpdatedAt = rt.UpdatedAt
                };
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetUserEventsResponse> GetUserEvents(GetUserEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _registerTeamRepository.GetApprovedByUserIdAsync(
            request.UserId, request.Keyword, request.PageIndex, request.PageSize);

        return new GetUserEventsResponse
        {
            Events = items.Select(rt => new UserEventItem
            {
                RegisterTeamId = rt.Id,
                Status = rt.Status?.ToString(),
                IsBanned = rt.IsBanned,
                IsDisable = rt.IsDisable,
                CreatedAt = rt.CreatedAt,
                UpdatedAt = rt.UpdatedAt,

                // Event
                EventId = rt.EventId,
                EventName = rt.Event?.Name,
                EventDescription = rt.Event?.Description,
                EventStartTime = rt.Event?.StartTime,
                EventEndTime = rt.Event?.EndTime,
                EventStatus = rt.Event?.Status?.ToString(),

                // Team
                TeamId = rt.TeamId,
                TeamName = rt.Team?.Name,

                // Track / Topic
                TrackId = rt.TrackId,
                TrackTitle = rt.Track?.Title,
                TopicId = rt.TopicId,
                TopicTitle = rt.Topic?.Title
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task RejectRegisterTeam(Guid registerTeamId, string? rejectionReason)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.Status != RegisterTeamStatusEnum.Pending)
            throw new BadRequestException("Only Pending Register Team Can Be Rejected");

        // [Commented] Check event time window — bỏ check để dễ test
        //var ev = await _eventRepository.GetByIdAsync(rt.EventId);
        //if (ev != null)
        //{
        //    if (ev.StartTime.HasValue && DateTimeOffset.UtcNow < ev.StartTime.Value)
        //        throw new BadRequestException("Cannot Reject Before Event Starts");
        //    if (ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
        //        throw new BadRequestException("Cannot Reject After Event Has Ended");
        //}

        rt.Status = RegisterTeamStatusEnum.Rejected;
        if (rejectionReason != null)
            rt.RejectionReason = rejectionReason;

        // Mở khóa team — có thể chỉnh sửa lại
        // NHƯNG chỉ khi team ko còn registration pending/approved/banned ở event khác
        var team = await _teamRepository.GetByIdAsync(rt.TeamId);
        if (team != null)
        {
            var hasOtherActive = await _registerTeamRepository.HasOtherActiveRegistrationAsync(rt.TeamId, registerTeamId);
            if (!hasOtherActive)
            {
                team.CanEdit = true;
            }
            team.UpdatedAt = DateTimeOffset.UtcNow;
            await _teamRepository.UpdateAsync(team);
        }

        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team
        if (team != null && rt.Event != null)
        {
            var notification = NotificationHelper.Create(
                NotificationTargetTypeEnum.Team,
                "Registration Rejected",
                string.Format(NotificationMessage.RegisterEvent.Rejected, team.Name, rejectionReason ?? ""),
                teamId: rt.TeamId);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<AssignToNextRoundResponse> AssignToNextRound(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdWithRoundDetailsAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        // [Commented] Chỉ được up round khi event chưa kết thúc — bỏ check để dễ test
        //var ev = await _eventRepository.GetByIdAsync(rt.EventId);
        //if (ev != null && ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
        //    throw new BadRequestException("Cannot Assign To Next Round After Event Has Ended");

        // Lấy round hiện tại (có RoundNo cao nhất). Nếu chưa có → currentRoundNo = 0 → gán round No1
        var currentRoundDetail = rt.RoundDetails
            .OrderByDescending(rd => rd.Round?.RoundNo)
            .FirstOrDefault();

        int currentRoundNo = currentRoundDetail?.Round?.RoundNo ?? 0;

        // Tìm round tiếp theo: EventId + RoundNo hiện tại + 1
        var nextRound = await _roundRepository.GetByEventIdAndRoundNoAsync(rt.EventId, currentRoundNo + 1);
        if (nextRound == null)
            throw new BadRequestException("This Is The Last Round. Cannot Assign To Next Round");

        // Check trùng — team đã có round detail cho round này chưa
        if (rt.RoundDetails.Any(rd => rd.RoundId == nextRound.Id))
            throw new BadRequestException("Team Is Already Assigned To This Round");

        // Check limit team của round tiếp theo
        if (nextRound.LimitTeam.HasValue)
        {
            var currentTeamCount = await _roundRepository.CountTeamsInRoundAsync(nextRound.Id);
            if (currentTeamCount >= nextRound.LimitTeam.Value)
                throw new BadRequestException("Next Round Is Full. Cannot Assign More Teams.");
        }

        // Tạo round detail mới
        var roundDetail = new RoundDetails
        {
            Id = Guid.NewGuid(),
            RoundId = nextRound.Id,
            RegisterTeamId = registerTeamId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        await _roundRepository.AddRoundDetailAsync(roundDetail);

        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team
        if (rt.Team != null && rt.Event != null)
        {
            var notification = NotificationHelper.Create(
                NotificationTargetTypeEnum.Team,
                "Advanced To Next Round",
                string.Format(NotificationMessage.RegisterEvent.AdvancedToNextRound, rt.Team.Name, nextRound.RoundNo ?? 0),
                teamId: rt.TeamId);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        return new AssignToNextRoundResponse
        {
            RegisterTeamId = registerTeamId,
            EventId = rt.EventId,
            TeamId = rt.TeamId,
            TeamName = rt.Team?.Name,
            TrackId = rt.TrackId,
            TrackName = rt.Track?.Title,
            TopicId = rt.TopicId,
            TopicName = rt.Topic?.Title,
            RoundId = nextRound.Id,
            RoundName = nextRound.Name,
            RoundNo = nextRound.RoundNo ?? 0
        };
    }

    public async Task<AssignToNextRoundResponse> RevertToPreviousRound(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdWithRoundDetailsAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        // [Commented] Chỉ được down round khi event chưa kết thúc — bỏ check để dễ test
        //var ev = await _eventRepository.GetByIdAsync(rt.EventId);
        //if (ev != null && ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
        //    throw new BadRequestException("Cannot Revert To Previous Round After Event Has Ended");

        // Lấy các round detail đang active, sắp xếp giảm dần theo RoundNo
        var activeRounds = rt.RoundDetails
            .Where(rd => rd.Round != null && !rd.IsDisable)
            .OrderByDescending(rd => rd.Round!.RoundNo)
            .ToList();

        if (activeRounds.Count < 2)
            throw new BadRequestException("Team Is Only In One Round. Cannot Revert To Previous Round");

        // Round hiện tại = đầu danh sách (RoundNo cao nhất)
        var currentRoundDetail = activeRounds.First();

        // Nếu round hiện tại đã có submission → ko thể quay lại
        if (currentRoundDetail.Submissions.Count > 0)
            throw new BadRequestException("Cannot Revert: Current Round Has Submission(s). Please Delete Submissions First");

        // Xóa cứng round detail (ko có submission thì safe)
        await _roundRepository.DeleteRoundDetailHardAsync(currentRoundDetail);

        // Round trước đó = phần tử thứ 2 trong danh sách
        var previousRound = activeRounds[1].Round!;

        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team
        if (rt.Team != null && rt.Event != null)
        {
            var notification = NotificationHelper.Create(
                NotificationTargetTypeEnum.Team,
                "Moved Back To Previous Round",
                string.Format(NotificationMessage.RegisterEvent.MovedBackToPreviousRound, rt.Team.Name, previousRound.RoundNo ?? 0),
                teamId: rt.TeamId);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        return new AssignToNextRoundResponse
        {
            RegisterTeamId = registerTeamId,
            EventId = rt.EventId,
            TeamId = rt.TeamId,
            TeamName = rt.Team?.Name,
            TrackId = rt.TrackId,
            TrackName = rt.Track?.Title,
            TopicId = rt.TopicId,
            TopicName = rt.Topic?.Title,
            RoundId = previousRound.Id,
            RoundName = previousRound.Name,
            RoundNo = previousRound.RoundNo ?? 0
        };
    }

    public async Task BanRegisterTeam(Guid registerTeamId, string rejectionReason)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.IsBanned)
            throw new BadRequestException("Register Team Is Already Banned");

        if (rt.Status != RegisterTeamStatusEnum.Approved)
            throw new BadRequestException("Only Approved Register Team Can Be Banned");

        rt.IsBanned = true;
        rt.Status = RegisterTeamStatusEnum.Banned;
        rt.RejectionReason = rejectionReason;
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team
        if (rt.Team != null && rt.Event != null)
        {
            var notification = NotificationHelper.Create(
                NotificationTargetTypeEnum.Team,
                "Team Banned",
                string.Format(NotificationMessage.RegisterEvent.Banned, rt.Team.Name, rejectionReason),
                teamId: rt.TeamId);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task UnbanRegisterTeam(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (!rt.IsBanned)
            throw new BadRequestException("Register Team Is Not Banned");

        rt.IsBanned = false;
        rt.Status = RegisterTeamStatusEnum.Approved;
        rt.RejectionReason = null;
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team
        if (rt.Team != null && rt.Event != null)
        {
            var notification = NotificationHelper.Create(
                NotificationTargetTypeEnum.Team,
                "Team Unbanned",
                string.Format(NotificationMessage.RegisterEvent.Unbanned, rt.Team.Name),
                teamId: rt.TeamId);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task AssignTrackTopic(Guid registerTeamId, AssignTrackTopicRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        var track = await _trackRepository.GetByIdAsync(request.TrackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        if (track.EventId != rt.EventId)
            throw new BadRequestException("Track Does Not Belong To The Same Event");

        if (request.TopicId.HasValue)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId.Value);
            if (topic == null)
                throw new NotFoundException("Topic Not Found");

            if (topic.TrackId != request.TrackId)
                throw new BadRequestException("Topic Does Not Belong To The Specified Track");
        }

        rt.TrackId = request.TrackId;
        rt.TopicId = request.TopicId;
        rt.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team — ghi rõ track name và topic name
        var trackTitle = track.Title;
        var topicTitle = request.TopicId.HasValue ? (await _topicRepository.GetByIdAsync(request.TopicId.Value))?.Title : null;
        if (rt.Team != null && rt.Event != null)
        {
            var notif = NotificationHelper.Create(
                NotificationTargetTypeEnum.Team,
                "Track Assigned",
                string.Format(NotificationMessage.RegisterEvent.TrackAssigned, rt.Team.Name, trackTitle, rt.Event.Name),
                teamId: rt.TeamId);
            await _notificationRepository.AddAsync(notif);

            if (!string.IsNullOrEmpty(topicTitle))
            {
                var topicNotif = NotificationHelper.Create(
                    NotificationTargetTypeEnum.Team,
                    "Topic Assigned",
                    string.Format(NotificationMessage.RegisterEvent.TopicAssigned, rt.Team.Name, topicTitle, rt.Event.Name),
                    teamId: rt.TeamId);
                await _notificationRepository.AddAsync(topicNotif);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task RemoveTrackTopic(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        // Lưu track/topic name trước khi xoá FK để ghi notification
        var removedTrackName = rt.Track?.Title ?? "";
        var removedTopicName = rt.Topic?.Title ?? "";

        rt.TrackId = null;
        rt.TopicId = null;
        rt.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team — ghi rõ track và topic đã bị remove
        if (rt.Team != null && rt.Event != null)
        {
            var notification = NotificationHelper.Create(
                NotificationTargetTypeEnum.Team,
                "Track/Topic Removed",
                string.Format(NotificationMessage.RegisterEvent.TrackTopicRemoved, rt.Team.Name, removedTrackName, removedTopicName, rt.Event.Name),
                teamId: rt.TeamId);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
