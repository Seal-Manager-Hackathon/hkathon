using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.RegisterTeam;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.RegisterTeam;

public class Service : IRegisterTeamService
{
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IRegisterTeamRepository registerTeamRepository,
        ITeamRepository teamRepository,
        IRoundRepository roundRepository,
        ITrackRepository trackRepository,
        ITopicRepository topicRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _registerTeamRepository = registerTeamRepository;
        _teamRepository = teamRepository;
        _roundRepository = roundRepository;
        _trackRepository = trackRepository;
        _topicRepository = topicRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    private async Task EnsureStaffAssignedToEvent(Guid eventId)
    {
        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);
    }

    private async Task EnsureStaffAssignedToRegisterTeamEvent(Guid registerTeamId)
    {
        await StaffAssignmentHelper.ValidateAssignmentForRegisterTeamAsync(
            _registerTeamRepository, _assignEventRepository, _currentUserService, registerTeamId);
    }

    public async Task<GetRegisterTeamsResponse> GetRegisterTeams(Guid eventId, GetRegisterTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToEvent(eventId);

        request.EventId = eventId;
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

    public async Task<RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

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
            EventId = rt.EventId,
            EventName = rt.Event?.Name,
            EventDescription = rt.Event?.Description,
            EventStartDate = rt.Event?.StartTime,
            EventEndDate = rt.Event?.EndTime,
            TeamId = rt.TeamId,
            TeamName = rt.Team?.Name,
            TeamCanEdit = rt.Team?.CanEdit ?? false,
            TeamIsDisable = rt.Team?.IsDisable ?? false,
            TeamCreatedAt = rt.Team?.CreatedAt ?? default,
            TrackId = rt.TrackId,
            TrackTitle = rt.Track?.Title,
            TopicId = rt.TopicId,
            TopicTitle = rt.Topic?.Title,
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

    public async Task UpdateRegisterTeam(Guid registerTeamId, UpdateRegisterTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
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
        // Staff KHÔNG được sửa IsBanned, IsDisable — chỉ Admin mới có quyền này

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ApproveRegisterTeam(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.Status != RegisterTeamStatusEnum.Pending)
            throw new BadRequestException("Only Pending Register Team Can Be Approved");

        var firstRound = await _roundRepository.GetFirstRoundByEventIdAsync(rt.EventId);
        if (firstRound != null && firstRound.LimitTeam.HasValue)
        {
            var currentTeamCount = await _roundRepository.CountTeamsInRoundAsync(firstRound.Id);
            if (currentTeamCount >= firstRound.LimitTeam.Value)
                throw new BadRequestException("Round 1 Is Full. Cannot Approve More Teams");
        }

        rt.Status = RegisterTeamStatusEnum.Approved;

        var team = await _teamRepository.GetByIdAsync(rt.TeamId);
        if (team != null)
        {
            team.CanEdit = false;
            await _teamRepository.UpdateAsync(team);
        }

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
    }

    public async Task RejectRegisterTeam(Guid registerTeamId, string? rejectionReason)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.Status != RegisterTeamStatusEnum.Pending)
            throw new BadRequestException("Only Pending Register Team Can Be Rejected");

        rt.Status = RegisterTeamStatusEnum.Rejected;
        if (rejectionReason != null)
            rt.RejectionReason = rejectionReason;

        var hasOtherApproved = await _registerTeamRepository.HasOtherApprovedAsync(rt.TeamId, registerTeamId);
        var team = await _teamRepository.GetByIdAsync(rt.TeamId);
        if (team != null && !hasOtherApproved)
        {
            team.CanEdit = true;
            await _teamRepository.UpdateAsync(team);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetUserEventsResponse> GetUserEvents(Guid userId, GetUserEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        request.UserId = userId;
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
                EventId = rt.EventId,
                EventName = rt.Event?.Name,
                EventDescription = rt.Event?.Description,
                EventStartTime = rt.Event?.StartTime,
                EventEndTime = rt.Event?.EndTime,
                EventStatus = rt.Event?.Status?.ToString(),
                TeamId = rt.TeamId,
                TeamName = rt.Team?.Name,
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

    public async Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(Guid teamId, GetRegisterTeamsByTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        request.TeamId = teamId;
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

    public async Task BanRegisterTeam(Guid registerTeamId, string rejectionReason)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.IsBanned)
            throw new BadRequestException("Register Team Is Already Banned");

        rt.IsBanned = true;
        rt.Status = RegisterTeamStatusEnum.Banned;
        rt.RejectionReason = rejectionReason;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UnbanRegisterTeam(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (!rt.IsBanned)
            throw new BadRequestException("Register Team Is Not Banned");

        rt.IsBanned = false;
        rt.Status = RegisterTeamStatusEnum.Approved;
        rt.RejectionReason = null;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<AssignToNextRoundResponse> AssignToNextRound(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

        var rt = await _registerTeamRepository.GetByIdWithRoundDetailsAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        var currentRoundDetail = rt.RoundDetails
            .OrderByDescending(rd => rd.Round?.RoundNo)
            .FirstOrDefault();

        int currentRoundNo = currentRoundDetail?.Round?.RoundNo ?? 0;

        var nextRound = await _roundRepository.GetByEventIdAndRoundNoAsync(rt.EventId, currentRoundNo + 1);
        if (nextRound == null)
            throw new BadRequestException("This Is The Last Round. Cannot Assign To Next Round");

        if (rt.RoundDetails.Any(rd => rd.RoundId == nextRound.Id))
            throw new BadRequestException("Team Is Already Assigned To This Round");

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
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

        var rt = await _registerTeamRepository.GetByIdWithRoundDetailsAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        var activeRounds = rt.RoundDetails
            .Where(rd => rd.Round != null && !rd.IsDisable)
            .OrderByDescending(rd => rd.Round!.RoundNo)
            .ToList();

        if (activeRounds.Count < 2)
            throw new BadRequestException("Team Is Only In One Round. Cannot Revert To Previous Round");

        var currentRoundDetail = activeRounds.First();

        if (currentRoundDetail.Submissions.Count > 0)
            throw new BadRequestException("Cannot Revert: Current Round Has Submission(s). Please Delete Submissions First");

        await _roundRepository.DeleteRoundDetailHardAsync(currentRoundDetail);

        var previousRound = activeRounds[1].Round!;
        await _unitOfWork.SaveChangesAsync();

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

    public async Task AssignTrackTopic(Guid registerTeamId, AssignTrackTopicRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

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
    }

    public async Task RemoveTrackTopic(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToRegisterTeamEvent(registerTeamId);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        rt.TrackId = null;
        rt.TopicId = null;
        rt.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }
}
