using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Topic;

public class Service : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ITopicRepository topicRepository,
        ITrackRepository trackRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _topicRepository = topicRepository;
        _trackRepository = trackRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetTopicsResponse> GetTopics(Guid trackId, GetTopicsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            track.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var (items, totalCount) = await _topicRepository.SearchAsync(
            trackId, request.Keyword, isDisable: false,
            request.PageIndex, request.PageSize);

        var topicItems = items
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new StaffTopicItem
            {
                Id = t.Id,
                TrackId = t.TrackId,
                TrackTitle = t.Track?.Title ?? string.Empty,
                Title = t.Title,
                Description = t.Description,
                IsDisable = t.IsDisable,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToList();

        return new GetTopicsResponse
        {
            Items = topicItems,
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic == null || topic.IsDisable)
            throw new NotFoundException("Topic Not Found");

        var track = await _trackRepository.GetByIdAsync(topic.TrackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            track.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        return new GetTopicDetailResponse
        {
            Id = topic.Id,
            TrackId = topic.TrackId,
            TrackTitle = topic.Track?.Title ?? string.Empty,
            Title = topic.Title,
            Description = topic.Description,
            IsDisable = topic.IsDisable,
            CreatedAt = topic.CreatedAt,
            UpdatedAt = topic.UpdatedAt
        };
    }
}
