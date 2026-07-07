namespace Hackathon.Application.Services.Topic;

public interface ITopicService
{
    Task<CreateTopicResponse> CreateTopic(CreateTopicRequest request);
    Task<GetTopicsByTrackResponse> GetTopicsByTrack(GetTopicsByTrackRequest request);
    Task UpdateTopic(UpdateTopicRequest request);
}
