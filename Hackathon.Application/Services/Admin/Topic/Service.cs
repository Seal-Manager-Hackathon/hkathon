using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Topic;

public class Service : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        ITopicRepository topicRepository,
        ITrackRepository trackRepository,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _topicRepository = topicRepository;
        _trackRepository = trackRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTopicResponse> CreateTopic(CreateTopicRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var track = await _trackRepository.GetByIdAsync(request.TrackId);
        if (track == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Check duplicate title trong cùng track
        var existingTopics = await _topicRepository.GetByTrackIdAsync(request.TrackId);
        if (existingTopics.Any(t => t.Title.Equals(request.Title, StringComparison.OrdinalIgnoreCase)))
            throw new BadRequestException("Topic Title Already Exists In This Track");

        var now = DateTimeOffset.UtcNow;
        var topic = new Topics
        {
            Id = Guid.NewGuid(),
            TrackId = request.TrackId,
            Title = request.Title,
            Description = request.Description,
            IsDisable = false,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _topicRepository.AddAsync(topic);
        await _unitOfWork.SaveChangesAsync();

        return new CreateTopicResponse
        {
            Id = topic.Id,
            TrackId = topic.TrackId,
            Title = topic.Title,
            Description = topic.Description,
            IsDisable = topic.IsDisable
        };
    }

    public async Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

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

    public async Task<GetTopicsByTrackResponse> GetTopicsByTrack(GetTopicsByTrackRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

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

    public async Task UpdateTopic(UpdateTopicRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var topic = await _topicRepository.GetByIdAsync(request.TopicId);
        if (topic == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        if (request.Title != null)
            topic.Title = request.Title;
        if (request.Description != null)
            topic.Description = request.Description;
        if (request.IsDisable.HasValue)
            topic.IsDisable = request.IsDisable.Value;

        topic.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }
}
