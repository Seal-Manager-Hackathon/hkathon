namespace Hackathon.Application.Services.Staff.Topic;

public interface ITopicService
{
    Task<GetTopicsResponse> GetTopics(Guid trackId, GetTopicsRequest request);
    Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId);
}
