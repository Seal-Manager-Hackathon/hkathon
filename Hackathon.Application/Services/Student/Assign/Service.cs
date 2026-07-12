using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Assign;

public class Service : IAssignService
{
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IAssignEventRepository assignEventRepository,
        IAuthorizationService authorizationService)
    {
        _assignEventRepository = assignEventRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetAssignedUsersResponse> GetAssignedUsers(GetAssignedUsersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        Domain.Enums.EventRole.EventRoleEnum? eventRole = null;
        if (!string.IsNullOrWhiteSpace(request.EventRole))
        {
            if (!Enum.TryParse<Domain.Enums.EventRole.EventRoleEnum>(request.EventRole, true, out var parsed))
                throw new BadRequestException("Invalid EventRole. Must be: Mentor, Judge, Staff");
            eventRole = parsed;
        }

        var (items, totalCount) = await _assignEventRepository.GetAssignedUsersByEventAsync(
            request.EventId, request.Keyword, eventRole, request.PageIndex, request.PageSize);

        // Student: chi lay user co IsDisable = false
        var filteredItems = items.Where(ae => !ae.IsDisable).ToList();
        var filteredTotalCount = filteredItems.Count;

        return new GetAssignedUsersResponse
        {
            Items = filteredItems.Select(ae => new AssignedUserItem
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
                    .Where(at => !at.IsDisable)
                    .Select(at => new AssignedTrackItem
                    {
                        TrackId = at.TrackId,
                        Title = at.Track.Title,
                        EventId = ae.EventId,
                        IsDisable = at.IsDisable
                    }).ToList()
            }).ToList(),
            TotalCount = filteredTotalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
