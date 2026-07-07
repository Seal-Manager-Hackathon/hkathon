using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.Admin.Team;

public class Service : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(ITeamRepository teamRepository, IAuthorizationService authorizationService, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetTeamsResponse> GetTeams(GetTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _teamRepository.SearchAsync(
            request.Keyword, request.CanEdit,
            request.FromDate, request.ToDate, request.IsDisable,
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

    public async Task UpdateTeam(UpdateTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var team = await _teamRepository.GetByIdAsync(request.TeamId);
        if (team == null)
            throw new NotFoundException("Team Not Found");

        if (request.Name != null)
            team.Name = request.Name;
        if (request.CanEdit.HasValue)
            team.CanEdit = request.CanEdit.Value;
        if (request.IsDisable.HasValue)
            team.IsDisable = request.IsDisable.Value;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteTeam(Guid teamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new NotFoundException("Team Not Found");

        if (team.IsDisable)
            throw new BadRequestException("Team Is Already Disabled");

        team.IsDisable = true;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreTeam(Guid teamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new NotFoundException("Team Not Found");

        team.IsDisable = false;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

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

    public async Task<GetUserTeamsResponse> GetUserTeams(GetUserTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        TeamDetailStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<TeamDetailStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Active, Inactive");
            status = parsed;
        }

        var (items, totalCount) = await _teamRepository.GetUserTeamsAsync(
            request.UserId, request.Keyword, status, request.IsDisable,
            request.PageIndex, request.PageSize);

        return new GetUserTeamsResponse
        {
            Teams = items.Select(td => new UserTeamItem
            {
                TeamDetailId = td.Id,
                TeamId = td.TeamId,
                TeamName = td.Team?.Name,
                CanEdit = td.Team?.CanEdit ?? false,
                IsDisable = td.Team?.IsDisable ?? false,
                IsLeader = td.IsLeader,
                Status = td.Status?.ToString(),
                CreatedAt = td.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var total = await _teamRepository.CountAsync(request.IsDisable);

        return new GetTeamCountResponse
        {
            Total = total
        };
    }
}
