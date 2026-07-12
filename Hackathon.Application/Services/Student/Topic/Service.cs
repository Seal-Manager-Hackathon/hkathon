using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Topic;

public class Service : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly ITrackRepository _trackRepository;

    public Service(
        ITopicRepository topicRepository,
        ITrackRepository trackRepository)
    {
        _topicRepository = topicRepository;
        _trackRepository = trackRepository;
    }

    public async Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic == null || topic.IsDisable)
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

    public async Task<GetTopicsByTrackResponse> GetTopicsByTrack(GetTopicsByTrackRequest request)
    {
        var track = await _trackRepository.GetByIdAsync(request.TrackId);
        if (track == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allTopics = await _topicRepository.GetByTrackIdAsync(request.TrackId);

        // Student: only non-disabled topics
        var query = allTopics.Where(t => !t.IsDisable).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
        }

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
}
