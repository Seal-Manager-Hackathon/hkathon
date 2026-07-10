using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Assign;

public class Service : IAssignService
{
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetAssignedUsersResponse> GetAssignedUsers(Guid eventId, GetAssignedUsersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Lecturer must be assigned to this event to view assigned users
        var assignment = await _assignEventRepository.GetByEventIdAndUserIdAsync(eventId, currentUserId.Value);
        if (assignment == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        Domain.Enums.EventRole.EventRoleEnum? eventRole = null;
        if (!string.IsNullOrWhiteSpace(request.EventRole))
        {
            if (!Enum.TryParse<Domain.Enums.EventRole.EventRoleEnum>(request.EventRole, true, out var parsed))
                throw new BadRequestException("Invalid EventRole. Must be: Mentor, Judge, Staff");
            eventRole = parsed;
        }

        var (items, totalCount) = await _assignEventRepository.GetAssignedUsersByEventAsync(
            eventId, request.Keyword, eventRole,
            request.PageIndex, request.PageSize);

        return new GetAssignedUsersResponse
        {
            Items = items
                .Where(ae => !ae.IsDisable)
                .Select(ae => new AssignedUserItem
                {
                    AssignEventId = ae.Id,
                    UserId = ae.User.Id,
                    Email = ae.User.Email,
                    FirstName = ae.User.FirstName,
                    LastName = ae.User.LastName,
                    AvatarUrl = string.IsNullOrEmpty(ae.User.AvatarUrl) ? null : ae.User.AvatarUrl,
                    EventRole = ae.EventRole?.Name.ToString(),
                    IsDisable = ae.IsDisable,
                    AssignTracks = ae.AssignTracks
                        .Select(at => new AssignedTrackItem
                        {
                            TrackId = at.TrackId,
                            Title = at.Track.Title,
                            EventId = ae.EventId,
                            IsDisable = at.IsDisable
                        }).ToList()
                }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
