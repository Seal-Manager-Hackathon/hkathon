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
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        ITeamRepository teamRepository,
        IRegisterTeamRepository registerTeamRepository,
        IUserRepository userRepository,
        IAuthorizationService authorizationService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _registerTeamRepository = registerTeamRepository;
        _userRepository = userRepository;
        _authorizationService = authorizationService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTeamResponse> CreateTeam(CreateTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        // Check user has completed profile
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);
        StudentProfileHelper.ValidateProfile(user);

        // Check duplicate team name
        var existingTeam = await _teamRepository.GetByNameAsync(request.Name);
        if (existingTeam != null)
            throw new BadRequestException("Team Name Already Exists");

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
            member.Status = TeamDetailStatusEnum.Inactive;
            member.UpdatedAt = now;
        }

        // Disable the team
        team.IsDisable = true;
        team.CanEdit = false;
        team.UpdatedAt = now;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task KickMember(Guid teamId, Guid memberId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        if (!team.CanEdit)
            throw new BadRequestException("Team Cannot Be Edited");

        // Check user is leader (active leader)
        var members = await _teamRepository.GetTeamMembersAsync(teamId);
        var leaderMember = members.FirstOrDefault(m => m.UserId == userId && m.IsLeader && !m.IsDisable);
        if (leaderMember == null)
            throw new BadRequestException("Only Team Leader Can Kick Members");

        // Can't kick yourself
        if (memberId == userId)
            throw new BadRequestException("Cannot Kick Yourself from the Team");

        // Find the member to kick
        var targetMember = members.FirstOrDefault(m => m.UserId == memberId && m.TeamId == teamId);
        if (targetMember == null)
            throw new NotFoundException("Member Not Found in This Team");

        if (targetMember.IsDisable || targetMember.Status == TeamDetailStatusEnum.Inactive)
            throw new BadRequestException("Member Is Already Inactive or Disabled");

        // Can't kick a leader
        if (targetMember.IsLeader)
            throw new BadRequestException("Cannot Kick the Team Leader");

        var now = DateTimeOffset.UtcNow;
        targetMember.IsDisable = true;
        targetMember.Status = TeamDetailStatusEnum.Inactive;
        targetMember.UpdatedAt = now;

        await _teamRepository.UpdateTeamDetailAsync(targetMember);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetTeamMembersResponse> GetTeamMembers(Guid teamId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(pageIndex, pageSize);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        var (items, totalCount) = await _teamRepository.GetTeamMembersPagedAsync(teamId, pageIndex, pageSize);

        // Student: chi lay member co Status = Active va IsDisable = false
        var filteredItems = items
            .Where(m => !m.IsDisable && m.Status == TeamDetailStatusEnum.Active)
            .ToList();

        // Tinh totalDisable tu tat ca member (ko chi trong page)
        var allMembers = await _teamRepository.GetTeamMembersAsync(teamId);
        var totalDisable = allMembers.Count(m => m.IsDisable || m.Status == TeamDetailStatusEnum.Inactive);

        return new GetTeamMembersResponse
        {
            TotalCount = filteredItems.Count,
            TotalDisable = totalDisable,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Members = filteredItems.Select(m => new TeamMemberItem
            {
                UserId = m.UserId,
                Email = m.User?.Email ?? "",
                FirstName = m.User?.FirstName ?? "",
                LastName = m.User?.LastName ?? "",
                AvatarUrl = m.User?.AvatarUrl,
                IsLeader = m.IsLeader,
                IsDisable = m.IsDisable,
                Status = m.Status?.ToString()
            }).ToList()
        };
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

    public async Task ChangeLeader(Guid teamId, Guid newLeaderUserId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        if (!team.CanEdit)
            throw new BadRequestException("Team Cannot Be Edited");

        // Can't transfer to yourself
        if (newLeaderUserId == userId)
            throw new BadRequestException("Cannot Transfer Leadership to Yourself");

        var members = await _teamRepository.GetTeamMembersAsync(teamId);

        // Check current user is the leader
        var currentLeader = members.FirstOrDefault(m => m.UserId == userId && m.IsLeader && !m.IsDisable);
        if (currentLeader == null)
            throw new BadRequestException("Only Team Leader Can Change Leader");

        // Check target user exists and is active
        var newLeader = members.FirstOrDefault(m => m.UserId == newLeaderUserId && m.TeamId == teamId);
        if (newLeader == null)
            throw new NotFoundException("User Not Found in This Team");

        if (newLeader.IsDisable || newLeader.Status == TeamDetailStatusEnum.Inactive)
            throw new BadRequestException("Cannot Transfer Leadership to an Inactive or Disabled Member");

        var now = DateTimeOffset.UtcNow;

        // Remove leader from current leader
        currentLeader.IsLeader = false;
        currentLeader.UpdatedAt = now;

        // Set new leader
        newLeader.IsLeader = true;
        newLeader.UpdatedAt = now;

        await _teamRepository.UpdateTeamDetailAsync(currentLeader);
        await _teamRepository.UpdateTeamDetailAsync(newLeader);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task LeaveTeam(Guid teamId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        var members = await _teamRepository.GetTeamMembersAsync(teamId);
        var myMember = members.FirstOrDefault(m => m.UserId == userId && m.TeamId == teamId);
        if (myMember == null)
            throw new NotFoundException("You Are Not a Member of This Team");

        if (myMember.IsDisable || myMember.Status == TeamDetailStatusEnum.Inactive)
            throw new BadRequestException("You Are Already Inactive or Disabled in This Team");

        // Leader cannot leave — must disband or change leader first
        if (myMember.IsLeader)
            throw new BadRequestException("Team Leader Cannot Leave the Team. Please Change Leader First or Disband the Team.");

        var now = DateTimeOffset.UtcNow;
        myMember.IsDisable = true;
        myMember.Status = TeamDetailStatusEnum.Inactive;
        myMember.UpdatedAt = now;

        await _teamRepository.UpdateTeamDetailAsync(myMember);
        await _unitOfWork.SaveChangesAsync();
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
