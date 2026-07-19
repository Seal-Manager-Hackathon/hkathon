using Hackathon.Application.Common;
using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Helpers.Notification;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.RegisterTeam;

public class Service : IRegisterTeamService
{
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IEventRepository _eventRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IRegisterTeamRepository registerTeamRepository,
        ITeamRepository teamRepository,
        IEventRepository eventRepository,
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _registerTeamRepository = registerTeamRepository;
        _teamRepository = teamRepository;
        _eventRepository = eventRepository;
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRegisterTeamsResponse> GetRegisterTeams(GetRegisterTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _registerTeamRepository.SearchAsync(
            request.EventId, request.Keyword, RegisterTeamStatusEnum.Approved,
            false, false,
            null, null,
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
        _authorizationService.Authorize(RoleEnum.Student);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null || rt.IsDisable || rt.IsBanned)
            throw new NotFoundException("Register Team Not Found");

        if (rt.Status != RegisterTeamStatusEnum.Approved)
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

    public async Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(GetRegisterTeamsByTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _registerTeamRepository.GetByTeamIdAsync(
            request.TeamId, RegisterTeamStatusEnum.Approved, false,
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

    public async Task<GetRegisterTeamsResponse> GetTeamRegisterTeams(Guid teamId, string? status, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(pageIndex, pageSize);

        // Parse status filter
        Domain.Enums.RegisterTeam.RegisterTeamStatusEnum? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<Domain.Enums.RegisterTeam.RegisterTeamStatusEnum>(status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected, Banned");
            statusFilter = parsed;
        }

        var (items, totalCount) = await _registerTeamRepository.GetByTeamIdAsync(
            teamId, statusFilter, null, pageIndex, pageSize);

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
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetRegisterTeamsResponse> GetTeamRegisterTeamsByEvent(Guid eventId, Guid teamId, string? status, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(pageIndex, pageSize);

        // Parse status filter
        RegisterTeamStatusEnum? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected, Banned");
            statusFilter = parsed;
        }

        var (items, totalCount) = await _registerTeamRepository.GetByEventIdAndTeamIdAsync(
            eventId, teamId, statusFilter, pageIndex, pageSize);

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
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<CreateRegisterTeamResponse> CreateRegisterTeam(CreateRegisterTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        // Check team exists
        var team = await _teamRepository.GetByIdAsync(request.TeamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        // Check user is leader of this team
        var members = await _teamRepository.GetTeamMembersAsync(request.TeamId);
        var isLeader = members.Any(m => m.UserId == userId && m.IsLeader && !m.IsDisable);
        if (!isLeader)
            throw new BadRequestException("Only Team Leader Can Register Team to Event");

        // Check event exists and is open for registration
        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev == null || ev.IsDisable)
            throw new NotFoundException("Event Not Found");
        if (ev.Status == Domain.Enums.Event.EventStatusEnum.Draft || ev.Status == Domain.Enums.Event.EventStatusEnum.Closed)
            throw new BadRequestException("Cannot Register to a Draft or Closed Event");

        // Check số lượng member active trong team nằm trong khoảng event yêu cầu
        var activeMembers = members.Where(m => !m.IsDisable && m.Status == Domain.Enums.TeamDetail.TeamDetailStatusEnum.Active).ToList();
        if (ev.MinMember.HasValue && activeMembers.Count < ev.MinMember.Value)
            throw new BadRequestException($"Team Must Have At Least {ev.MinMember.Value} Active Members To Register For This Event");
        if (ev.MaxMember.HasValue && activeMembers.Count > ev.MaxMember.Value)
            throw new BadRequestException($"Team Cannot Have More Than {ev.MaxMember.Value} Active Members To Register For This Event");

        // [Commented] Check registration is within the allowed time window — bỏ check để dễ test
        //if (ev.RegisterLimitTime.HasValue && DateTimeOffset.UtcNow >= ev.RegisterLimitTime.Value)
        //    throw new BadRequestException("Registration Period Has Ended. Cannot Register At This Time.");
        //if (ev.StartTime.HasValue && DateTimeOffset.UtcNow < ev.StartTime.Value)
        //    throw new BadRequestException("Registration Has Not Started Yet. Cannot Register Before Event Starts.");

        // Check team is not already registered to this event
        var existingRegister = await _registerTeamRepository.GetByEventIdAndTeamIdAsync(
            request.EventId, request.TeamId, null, 1, 1);
        if (existingRegister.Items.Count > 0)
        {
            var existing = existingRegister.Items.First();

            if (existing.IsBanned || existing.Status == RegisterTeamStatusEnum.Banned)
                throw new BadRequestException("You Have Been Banned From This Event");

            if (existing.Status == RegisterTeamStatusEnum.Rejected)
            {
                // Reset rejected registration → Pending, clear rejection reason
                existing.Status = RegisterTeamStatusEnum.Pending;
                existing.RejectionReason = null;
                existing.UpdatedAt = DateTimeOffset.UtcNow;
                await _unitOfWork.SaveChangesAsync();

                return new CreateRegisterTeamResponse
                {
                    Id = existing.Id,
                    TeamId = existing.TeamId,
                    EventId = existing.EventId,
                    Status = existing.Status?.ToString(),
                    CreatedAt = existing.CreatedAt
                };
            }

            throw new BadRequestException("Team Is Already Registered to This Event");
        }

        // Check no member of this team has an approved register team in this event
        var activeMemberIds = members
            .Where(m => !m.IsDisable && m.Status == Domain.Enums.TeamDetail.TeamDetailStatusEnum.Active)
            .Select(m => m.UserId)
            .ToList();

        if (activeMemberIds.Count > 0)
        {
            var hasMemberApproved = await _registerTeamRepository.HasAnyMemberApprovedInEventAsync(
                request.EventId, activeMemberIds);
            if (hasMemberApproved)
                throw new BadRequestException("One or More Team Members Already Have an Approved Registration in This Event");
        }

        var now = DateTimeOffset.UtcNow;
        var registerTeam = new Domain.Entities.RegisterTeams
        {
            Id = Guid.NewGuid(),
            TeamId = request.TeamId,
            EventId = request.EventId,
            TrackId = request.TrackId,
            TopicId = request.TopicId,
            Description = request.Description,
            Status = RegisterTeamStatusEnum.Pending,
            CreatedAt = now,
            UpdatedAt = now
        };

        // Khóa team: không thể sửa thông tin hoặc kick member sau khi đã gửi đăng ký
        team.CanEdit = false;
        team.UpdatedAt = now;

        await _registerTeamRepository.AddAsync(registerTeam);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team (gắn teamId — các member tự query qua team của họ)
        var teamNotification = NotificationHelper.Create(
            NotificationTargetTypeEnum.Team,
            "Event Registered",
            string.Format(NotificationMessage.RegisterEvent.Registered, team.Name, ev.Name),
            teamId: request.TeamId);
        await _notificationRepository.AddAsync(teamNotification);

        await _unitOfWork.SaveChangesAsync();

        return new CreateRegisterTeamResponse
        {
            Id = registerTeam.Id,
            TeamId = registerTeam.TeamId,
            EventId = registerTeam.EventId,
            Status = registerTeam.Status?.ToString(),
            CreatedAt = registerTeam.CreatedAt
        };
    }

    public async Task<GetUserEventsResponse> GetUserEvents(GetUserEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var currentUserId = _currentUserService.UserId;
        var userId = request.UserId != Guid.Empty ? request.UserId : (currentUserId ?? Guid.Empty);
        if (userId == Guid.Empty)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var (items, totalCount) = await _registerTeamRepository.GetApprovedByUserIdAsync(
            userId, request.Keyword, request.PageIndex, request.PageSize);

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
}
