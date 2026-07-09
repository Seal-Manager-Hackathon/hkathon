using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using Hackathon.Domain.Enums.EventRole;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Assign;

public class Service : IAssignService
{
    private readonly IUserRepository _userRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IUserRepository userRepository,
        IAssignEventRepository assignEventRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _userRepository = userRepository;
        _assignEventRepository = assignEventRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetAvailableLecturersResponse> GetAvailableLecturers(Guid eventId, GetAvailableLecturersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);

        var (items, totalCount) = await _userRepository.GetAvailableUsersByRoleAsync(
            eventId, RoleEnum.Lecturer, request.Keyword, request.PageIndex, request.PageSize);

        return new GetAvailableLecturersResponse
        {
            Items = items.Select(u => new AvailableLecturerItem
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarUrl = string.IsNullOrEmpty(u.AvatarUrl) ? null : u.AvatarUrl,
                College = string.IsNullOrEmpty(u.College) ? null : u.College,
                PhoneNumber = string.IsNullOrEmpty(u.PhoneNumber) ? null : u.PhoneNumber
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task AssignLecturerToEvent(Guid eventId, AssignLecturerToEventRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        if (user.Role != RoleEnum.Lecturer)
            throw new BadRequestException("Can Only Assign Lecturer To Event");

        if (!Enum.TryParse<EventRoleEnum>(request.EventRole, true, out var eventRoleEnum))
            throw new BadRequestException("Invalid EventRole. Must be: Judge, Mentor");

        if (eventRoleEnum == EventRoleEnum.Staff)
            throw new BadRequestException("Staff Cannot Assign Staff Role");

        var existing = await _assignEventRepository.GetByEventIdAndUserIdAsync(eventId, request.UserId);
        if (existing != null)
            throw new ConflictException("User Is Already Assigned To This Event");

        var eventRole = await _assignEventRepository.GetEventRoleByNameAsync(eventRoleEnum);
        if (eventRole == null)
            throw new NotFoundException($"Event Role {request.EventRole} Not Found");

        var assignEvent = new Hackathon.Domain.Entities.AssignEvents
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            UserId = request.UserId,
            EventRoleId = eventRole.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _assignEventRepository.Add(assignEvent);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetAssignedUsersResponse> GetAssignedUsers(Guid eventId, GetAssignedUsersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);

        Domain.Enums.EventRole.EventRoleEnum? eventRole = null;
        if (!string.IsNullOrWhiteSpace(request.EventRole))
        {
            if (!Enum.TryParse<Domain.Enums.EventRole.EventRoleEnum>(request.EventRole, true, out var parsed))
                throw new BadRequestException("Invalid EventRole. Must be: Mentor, Judge, Staff");
            eventRole = parsed;
        }

        Domain.Enums.User.RoleEnum? role = null;
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!Enum.TryParse<Domain.Enums.User.RoleEnum>(request.Role, true, out var parsed))
                throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
            role = parsed;
        }

        var (items, totalCount) = await _assignEventRepository.GetAssignedUsersByEventAsync(
            eventId, request.Keyword, eventRole, role, request.TrackId,
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
                    AssignTracks = ae.AssignTracks
                        .Where(at => !at.IsDisable)
                        .Select(at => new AssignedTrackItem
                        {
                            TrackId = at.TrackId,
                            Title = at.Track.Title,
                            EventId = ae.EventId
                        }).ToList()
                }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetAssignEventDetailResponse> GetAssignEventDetail(Guid eventId, Guid assignEventId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null || assignEvent.IsDisable)
            throw new NotFoundException("Assign Event Not Found");

        return new GetAssignEventDetailResponse
        {
            Id = assignEvent.Id,
            EventId = assignEvent.EventId,
            UserId = assignEvent.User.Id,
            Email = assignEvent.User.Email,
            FirstName = assignEvent.User.FirstName,
            LastName = assignEvent.User.LastName,
            AvatarUrl = string.IsNullOrEmpty(assignEvent.User.AvatarUrl) ? null : assignEvent.User.AvatarUrl,
            EventRole = assignEvent.EventRole?.Name.ToString(),
            Tracks = assignEvent.AssignTracks
                .Where(at => !at.IsDisable)
                .Select(at => new AssignedTrackItem
                {
                    TrackId = at.TrackId,
                    Title = at.Track.Title,
                    EventId = assignEvent.EventId
                }).ToList(),
            CreatedAt = assignEvent.CreatedAt,
            UpdatedAt = assignEvent.UpdatedAt
        };
    }
}
