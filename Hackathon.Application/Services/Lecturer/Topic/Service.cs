using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Topic;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Topic;

public class Service : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ITopicRepository topicRepository,
        ITrackRepository trackRepository,
        IAuthorizationService authorizationService)
    {
        _topicRepository = topicRepository;
        _trackRepository = trackRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetTopicsByTrackResponse> GetTopicsByTrack(GetTopicsByTrackRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var track = await _trackRepository.GetByIdAsync(request.TrackId);
        if (track == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allTopics = await _topicRepository.GetByTrackIdAsync(request.TrackId);

        var query = allTopics.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
        }

        if (request.IsDisable.HasValue)
            query = query.Where(t => t.IsDisable == request.IsDisable.Value);

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new TopicItem
            {
                Id = t.Id,
                TrackId = t.TrackId,
                TrackTitle = t.Track.Title,
                Title = t.Title,
                Description = t.Description,
                IsDisable = t.IsDisable,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToList();

        return new GetTopicsByTrackResponse
        {
            Topics = items,
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return new GetTopicDetailResponse
        {
            Id = topic.Id,
            TrackId = topic.TrackId,
            TrackTitle = topic.Track?.Title ?? "",
            Title = topic.Title,
            Description = topic.Description,
            IsDisable = topic.IsDisable,
            CreatedAt = topic.CreatedAt,
            UpdatedAt = topic.UpdatedAt
        };
    }
}
