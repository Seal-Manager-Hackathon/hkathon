using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Staff.RegisterTeam;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.RegisterTeam;

public class Service : IRegisterTeamService
{
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRegisterTeamRepository registerTeamRepository,
        ITeamRepository teamRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _registerTeamRepository = registerTeamRepository;
        _teamRepository = teamRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    private async Task EnsureLecturerAssignedToEvent(Guid eventId)
    {
        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignment = await _assignEventRepository.GetByEventIdAndUserIdAsync(eventId, currentUserId.Value);
        if (assignment == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");
    }

    public async Task<GetRegisterTeamsResponse> GetRegisterTeams(Guid eventId, GetRegisterTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);
        await EnsureLecturerAssignedToEvent(eventId);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RegisterTeamStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected, Banned");
            status = parsed;
        }

        // Lecturer only sees active register teams (IsDisable = false)
        var (items, totalCount) = await _registerTeamRepository.SearchAsync(
            eventId, request.Keyword, status,
            request.IsBanned, false,
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
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        await EnsureLecturerAssignedToEvent(rt.EventId);

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

    public async Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(Guid teamId, GetRegisterTeamsByTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RegisterTeamStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected, Banned");
            status = parsed;
        }

        var (items, totalCount) = await _registerTeamRepository.GetByTeamIdAsync(
            teamId, status, request.IsDisable, request.PageIndex, request.PageSize);

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
}
