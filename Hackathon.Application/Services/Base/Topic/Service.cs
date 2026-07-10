using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Topic;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.Topic;

public class Service : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ITopicRepository topicRepository,
        IAuthorizationService authorizationService)
    {
        _topicRepository = topicRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId)
    {
        _authorizationService.Authenticate();

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
