using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Team;

public class Service : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ITeamRepository teamRepository,
        IRegisterTeamRepository registerTeamRepository,
        IAuthorizationService authorizationService)
    {
        _teamRepository = teamRepository;
        _registerTeamRepository = registerTeamRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetTeamsResponse> GetTeams(GetTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        // Lecturer: luôn lọc IsDisable = false
        var (items, totalCount) = await _teamRepository.SearchAsync(
            request.Keyword, request.CanEdit,
            request.FromDate, request.ToDate, false,
            request.PageIndex, request.PageSize);

        return new GetTeamsResponse
        {
            Teams = items.Select(t => new TeamCard
            {
                Id = t.Id,
                Name = t.Name,
                CanEdit = t.CanEdit,
                IsDisable = t.IsDisable,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new NotFoundException("Team Not Found");

        var members = await _teamRepository.GetTeamMembersAsync(teamId);

        return new GetTeamDetailResponse
        {
            Id = team.Id,
            Name = team.Name,
            CanEdit = team.CanEdit,
            IsDisable = team.IsDisable,
            CreatedAt = team.CreatedAt,
            UpdatedAt = team.UpdatedAt,
            Members = members.Select(m => new TeamMemberItem
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

    public async Task<GetTeamEventsResponse> GetTeamEvents(GetTeamEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _registerTeamRepository.GetByTeamIdAsync(
            request.TeamId, Domain.Enums.RegisterTeam.RegisterTeamStatusEnum.Approved, false,
            request.PageIndex, request.PageSize);

        return new GetTeamEventsResponse
        {
            Items = items.Select(rt => new TeamEventItem
            {
                RegisterTeamId = rt.Id,
                EventId = rt.EventId,
                EventName = rt.Event?.Name ?? "",
                Status = rt.Event?.Status?.ToString(),
                CreatedAt = rt.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var total = await _teamRepository.CountAsync(request.IsDisable);

        return new GetTeamCountResponse
        {
            Total = total
        };
    }

    public async Task<Admin.Team.GetRecentTeamsResponse> GetRecentTeams()
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var teams = await _teamRepository.GetRecentAsync(10);

        return new Admin.Team.GetRecentTeamsResponse
        {
            Teams = teams.Select(t => new Admin.Team.TeamCard
            {
                Id = t.Id,
                Name = t.Name,
                CanEdit = t.CanEdit,
                IsDisable = t.IsDisable,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList()
        };
    }
}