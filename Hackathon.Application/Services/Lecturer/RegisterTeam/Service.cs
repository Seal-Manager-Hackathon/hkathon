using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.RegisterTeam;

public class Service : IRegisterTeamService
{
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRegisterTeamRepository registerTeamRepository,
        ITeamRepository teamRepository,
        IRoundRepository roundRepository,
        ITrackRepository trackRepository,
        IAuthorizationService authorizationService)
    {
        _registerTeamRepository = registerTeamRepository;
        _teamRepository = teamRepository;
        _roundRepository = roundRepository;
        _trackRepository = trackRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetRegisterTeamsResponse> GetRegisterTeams(GetRegisterTeamsRequest request)
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
        _authorizationService.Authorize(RoleEnum.Lecturer);

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

    public async Task<GetUserEventsResponse> GetUserEvents(Guid userId, GetUserEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

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

    public async Task<GetRegisterTeamsByTrackResponse> GetRegisterTeamsByTrack(Guid trackId, Guid? roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(pageIndex, pageSize);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        var (items, totalCount) = await _registerTeamRepository.SearchAsync(
            eventId: track.EventId, keyword: null, status: null,
            isBanned: null, isDisable: null,
            fromDate: null, toDate: null,
            roundId: roundId, trackId: trackId, topicId: null,
            pageIndex, pageSize);

        return new GetRegisterTeamsByTrackResponse
        {
            RegisterTeams = items.Select(rt =>
            {
                var maxRound = rt.RoundDetails
                    .Where(rd => rd.Round != null && !rd.IsDisable)
                    .OrderByDescending(rd => rd.Round!.RoundNo)
                    .FirstOrDefault();

                return new RegisterTeamByTrackItem
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

    public async Task<GetCompetitionStatusResponse> GetCompetitionStatus(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        // Get max round detail of this team
        var maxRound = rt.RoundDetails
            .Where(rd => rd.Round != null && !rd.IsDisable)
            .OrderByDescending(rd => rd.Round!.RoundNo)
            .FirstOrDefault();

        // Determine current round based on UTC now
        var now = DateTimeOffset.UtcNow;
        var allRounds = await _roundRepository.SearchByEventIdAsync(
            rt.EventId, null, null, null, 1, int.MaxValue);
        var currentRound = allRounds.Items
            .Where(r => r.StartTime.HasValue && r.StartTime.Value <= now
                && (!r.EndTime.HasValue || r.EndTime.Value >= now))
            .OrderBy(r => r.RoundNo)
            .FirstOrDefault();

        var isStillCompeting = currentRound != null
            && maxRound?.Round?.RoundNo != null
            && maxRound.Round.RoundNo >= currentRound.RoundNo;

        return new GetCompetitionStatusResponse
        {
            RegisterTeamId = rt.Id,
            TeamId = rt.TeamId,
            TeamName = rt.Team?.Name,
            EventId = rt.EventId,
            EventName = rt.Event?.Name,
            TrackId = rt.TrackId,
            TrackName = rt.Track?.Title,
            TopicId = rt.TopicId,
            TopicName = rt.Topic?.Title,
            CurrentRoundId = currentRound?.Id,
            CurrentRoundName = currentRound?.Name,
            CurrentRoundNo = currentRound?.RoundNo,
            MaxRoundId = maxRound?.RoundId,
            MaxRoundName = maxRound?.Round?.Name,
            MaxRoundNo = maxRound?.Round?.RoundNo,
            IsStillCompeting = isStillCompeting
        };
    }
}