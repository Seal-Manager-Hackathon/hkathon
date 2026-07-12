namespace Hackathon.Application.Services.Student.Topic;

public interface ITopicService
{
    Task<GetTopicsByTrackResponse> GetTopicsByTrack(GetTopicsByTrackRequest request);
    Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId);
}
