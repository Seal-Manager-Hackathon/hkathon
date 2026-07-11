using Hackathon.Application.Services.Admin.Topic;

namespace Hackathon.Application.Services.Lecturer.Topic;

public interface ITopicService
{
    Task<GetTopicsByTrackResponse> GetTopicsByTrack(GetTopicsByTrackRequest request);
    Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId);
}
