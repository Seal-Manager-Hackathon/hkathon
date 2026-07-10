using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Team;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Team;

public class Service : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ITeamRepository teamRepository,
        IAuthorizationService authorizationService)
    {
        _teamRepository = teamRepository;
        _authorizationService = authorizationService;
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
}
