using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Team;

public class Service : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        ITeamRepository teamRepository,
        IRegisterTeamRepository registerTeamRepository,
        IAuthorizationService authorizationService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _registerTeamRepository = registerTeamRepository;
        _authorizationService = authorizationService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTeamResponse> CreateTeam(CreateTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var now = DateTimeOffset.UtcNow;
        var team = new Teams
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CanEdit = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _teamRepository.AddAsync(team);

        var teamDetail = new TeamDetails
        {
            Id = Guid.NewGuid(),
            TeamId = team.Id,
            UserId = userId,
            IsLeader = true,
            Status = TeamDetailStatusEnum.Active,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _teamRepository.AddTeamDetailAsync(teamDetail);
        await _unitOfWork.SaveChangesAsync();

        return new CreateTeamResponse
        {
            Id = team.Id,
            Name = team.Name,
            CanEdit = team.CanEdit
        };
    }

    public async Task UpdateTeam(UpdateTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var team = await _teamRepository.GetByIdAsync(request.TeamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        // Check user is leader
        var members = await _teamRepository.GetTeamMembersAsync(request.TeamId);
        var isLeader = members.Any(m => m.UserId == userId && m.IsLeader && !m.IsDisable);
        if (!isLeader)
            throw new BadRequestException("Only Team Leader Can Update Team");

        if (request.Name != null)
            team.Name = request.Name;

        team.UpdatedAt = DateTimeOffset.UtcNow;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetMyTeamsResponse> GetMyTeams(GetMyTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var (items, totalCount) = await _teamRepository.GetUserTeamsAsync(
            userId, request.Keyword, TeamDetailStatusEnum.Active, false,
            request.PageIndex, request.PageSize);

        var teamIds = items.Select(td => td.TeamId).ToList();
        var allMembers = await _teamRepository.GetTeamMembersAsync(teamIds.FirstOrDefault());

        return new GetMyTeamsResponse
        {
            Teams = items.Select(td => new MyTeamItem
            {
                TeamId = td.TeamId,
                TeamDetailId = td.Id,
                Name = td.Team?.Name ?? "",
                IsLeader = td.IsLeader,
                CanEdit = td.Team?.CanEdit ?? false,
                Status = td.Status?.ToString(),
                MemberCount = td.Team?.TeamDetails?.Count(td2 => !td2.IsDisable) ?? 0,
                CreatedAt = td.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task DisbandTeam(Guid teamId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        // Check user is leader
        var members = await _teamRepository.GetTeamMembersAsync(teamId);
        var leaderMember = members.FirstOrDefault(m => m.UserId == userId && m.IsLeader && !m.IsDisable);
        if (leaderMember == null)
            throw new BadRequestException("Only Team Leader Can Disband Team");

        var now = DateTimeOffset.UtcNow;

        // Disable all active members
        foreach (var member in members.Where(m => !m.IsDisable))
        {
            member.IsDisable = true;
            member.UpdatedAt = now;
        }

        // Disable the team
        team.IsDisable = true;
        team.CanEdit = false;
        team.UpdatedAt = now;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var total = await _teamRepository.CountAsync(false);

        return new GetTeamCountResponse
        {
            Total = total
        };
    }

    public async Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new NotFoundException("Team Not Found");

        if (team.IsDisable)
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
            Members = members
                .Where(m => !m.IsDisable)
                .Select(m => new TeamMemberItem
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
        _authorizationService.Authorize(RoleEnum.Student);

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
}
