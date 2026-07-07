namespace Hackathon.Application.Services.Admin.Topic;

public interface ITopicService
{
    Task<CreateTopicResponse> CreateTopic(CreateTopicRequest request);
    Task<GetTopicsByTrackResponse> GetTopicsByTrack(GetTopicsByTrackRequest request);
    Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId);
    Task UpdateTopic(UpdateTopicRequest request);
}
