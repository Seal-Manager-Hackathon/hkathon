using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Invitation;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.Admin.Invitation;

public class Service : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IInvitationRepository invitationRepository,
        ITeamRepository teamRepository,
        IAuthorizationService authorizationService)
    {
        _invitationRepository = invitationRepository;
        _teamRepository = teamRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetInvitationsResponse> GetInvitations(Guid teamId, string? status, string? keyword, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(pageIndex, pageSize);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new NotFoundException("Team Not Found");

        // Parse status filter
        InvitationStatusEnum? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<InvitationStatusEnum>(status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Accepted, Rejected, Expired");
            statusFilter = parsed;
        }

        var (items, totalCount) = await _invitationRepository.GetByTeamIdAsync(
            teamId, keyword, statusFilter, pageIndex, pageSize);

        var list = items.Select(i => new InvitationItem
        {
            Id = i.Id,
            TeamId = i.TeamId,
            TeamName = i.Team?.Name ?? "",
            UserId = i.UserId,
            UserEmail = i.User?.Email ?? "",
            UserFirstName = i.User?.FirstName ?? "",
            UserLastName = i.User?.LastName ?? "",
            UserAvatarUrl = i.User?.AvatarUrl,
            Status = i.Status?.ToString(),
            Description = i.Description,
            LimitTime = i.LimitTime,
            IsDisable = i.IsDisable,
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt
        }).ToList();

        return new GetInvitationsResponse
        {
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Items = list
        };
    }
}
