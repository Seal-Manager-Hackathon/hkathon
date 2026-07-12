using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Invitation;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Invitation;

public class Service : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IInvitationRepository invitationRepository,
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        IAuthorizationService authorizationService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _authorizationService = authorizationService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task SendInvitation(Guid teamId, string email)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        if (!team.CanEdit)
            throw new BadRequestException("Team Cannot Be Edited");

        var members = await _teamRepository.GetTeamMembersAsync(teamId);
        var leader = members.FirstOrDefault(m => m.UserId == userId && m.IsLeader && !m.IsDisable);
        if (leader == null)
            throw new BadRequestException("Only Team Leader Can Send Invitations");

        var invitedUser = await _userRepository.GetByEmailAsync(email.ToLower().Trim());
        if (invitedUser == null)
            throw new NotFoundException("User Not Found");

        if (invitedUser.IsDisable)
            throw new BadRequestException("Cannot Invite a Disabled User");

        var existingMember = members.FirstOrDefault(m => m.UserId == invitedUser.Id && !m.IsDisable && m.Status == TeamDetailStatusEnum.Active);
        if (existingMember != null)
            throw new BadRequestException("User Is Already a Member of This Team");

        var (existingInvitations, _) = await _invitationRepository.GetByTeamIdAsync(
            teamId, null, InvitationStatusEnum.Pending, 1, int.MaxValue);
        if (existingInvitations.Any(i => i.UserId == invitedUser.Id && i.Status == InvitationStatusEnum.Pending))
            throw new BadRequestException("An Invitation Has Already Been Sent to This User");

        var now = DateTimeOffset.UtcNow;
        var invitation = new Invitations
        {
            Id = Guid.NewGuid(),
            TeamId = teamId,
            UserId = invitedUser.Id,
            Status = InvitationStatusEnum.Pending,
            LimitTime = now.AddDays(15),
            CreatedAt = now,
            UpdatedAt = now
        };

        await _invitationRepository.AddAsync(invitation);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetInvitationsResponse> GetSentInvitations(Guid teamId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        var members = await _teamRepository.GetTeamMembersAsync(teamId);
        var leader = members.FirstOrDefault(m => m.UserId == userId && m.IsLeader && !m.IsDisable);
        if (leader == null)
            throw new BadRequestException("Only Team Leader Can View Sent Invitations");

        var (items, totalCount) = await _invitationRepository.GetByTeamIdAsync(
            teamId, null, null, pageIndex, pageSize);

        return new GetInvitationsResponse
        {
            Items = items.Select(i => new InvitationItem
            {
                Id = i.Id,
                TeamId = i.TeamId,
                TeamName = i.Team?.Name ?? "",
                TeamCanEdit = i.Team?.CanEdit ?? false,
                InvitedUserId = i.UserId,
                InvitedUserEmail = i.User?.Email,
                InvitedUserFirstName = i.User?.FirstName,
                InvitedUserLastName = i.User?.LastName,
                InvitedUserAvatarUrl = i.User?.AvatarUrl,
                Status = i.Status?.ToString(),
                Description = i.Description,
                LimitTime = i.LimitTime,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetInvitationsResponse> GetReceivedInvitations(string? keyword, string? status, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        PaginationHelper.Validate(pageIndex, pageSize);

        // Parse status filter
        InvitationStatusEnum? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<InvitationStatusEnum>(status, true, out var parsedStatus))
                throw new BadRequestException("Invalid Status. Must be: Pending, Accepted, Rejected, Expired");
            statusFilter = parsedStatus;
        }

        var (items, totalCount) = await _invitationRepository.GetByUserIdAsync(
            userId, keyword, statusFilter, pageIndex, pageSize);

        var mapped = items.Select(i =>
        {
            var leader = i.Team?.TeamDetails?
                .FirstOrDefault(td => td.IsLeader && !td.IsDisable);

            return new InvitationItem
            {
                Id = i.Id,
                TeamId = i.TeamId,
                TeamName = i.Team?.Name ?? "",
                TeamCanEdit = i.Team?.CanEdit ?? false,
                InvitedUserId = i.UserId,
                InvitedUserEmail = i.User?.Email,
                InvitedUserFirstName = i.User?.FirstName,
                InvitedUserLastName = i.User?.LastName,
                InvitedUserAvatarUrl = i.User?.AvatarUrl,
                SentByUserId = leader?.UserId,
                SentByEmail = leader?.User?.Email,
                SentByFirstName = leader?.User?.FirstName,
                SentByLastName = leader?.User?.LastName,
                SentByAvatarUrl = leader?.User?.AvatarUrl,
                Status = i.Status?.ToString(),
                Description = i.Description,
                LimitTime = i.LimitTime,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            };
        }).ToList();

        // Custom sort: Pending first (sorted by CreatedAt DESC), then others (sorted by CreatedAt DESC)
        var sorted = mapped
            .OrderByDescending(i => i.Status == "Pending" ? 1 : 0)
            .ThenByDescending(i => i.CreatedAt)
            .ToList();

        // Manual pagination after sort
        var paged = sorted
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new GetInvitationsResponse
        {
            Items = paged,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task AcceptInvitation(Guid invitationId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var invitation = await _invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
            throw new NotFoundException("Invitation Not Found");

        if (invitation.UserId != userId)
            throw new ForbiddenException("This Invitation Is Not for You");

        if (invitation.Status != InvitationStatusEnum.Pending)
            throw new BadRequestException("Invitation Is Not in Pending Status");

        // Check limit time
        if (invitation.LimitTime.HasValue && invitation.LimitTime.Value < DateTimeOffset.UtcNow)
        {
            invitation.Status = InvitationStatusEnum.Expired;
            invitation.UpdatedAt = DateTimeOffset.UtcNow;
            await _unitOfWork.SaveChangesAsync();
            throw new BadRequestException("Invitation Has Expired");
        }

        var team = invitation.Team;
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        // Check user is not already a member
        var members = await _teamRepository.GetTeamMembersAsync(invitation.TeamId);
        var existingMember = members.FirstOrDefault(m => m.UserId == userId && !m.IsDisable && m.Status == TeamDetailStatusEnum.Active);
        if (existingMember != null)
            throw new BadRequestException("You Are Already a Member of This Team");

        var now = DateTimeOffset.UtcNow;

        // Update invitation
        invitation.Status = InvitationStatusEnum.Accepted;
        invitation.UpdatedAt = now;

        // Add as team member
        var teamDetail = new TeamDetails
        {
            Id = Guid.NewGuid(),
            TeamId = invitation.TeamId,
            UserId = userId,
            IsLeader = false,
            Status = TeamDetailStatusEnum.Active,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _teamRepository.AddTeamDetailAsync(teamDetail);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RejectInvitation(Guid invitationId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var invitation = await _invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
            throw new NotFoundException("Invitation Not Found");

        if (invitation.UserId != userId)
            throw new ForbiddenException("This Invitation Is Not for You");

        if (invitation.Status != InvitationStatusEnum.Pending)
            throw new BadRequestException("Invitation Is Not in Pending Status");

        invitation.Status = InvitationStatusEnum.Rejected;
        invitation.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }
}
