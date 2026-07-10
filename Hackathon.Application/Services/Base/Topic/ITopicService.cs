using Hackathon.Application.Services.Admin.Topic;

namespace Hackathon.Application.Services.Base.Topic;

public interface ITopicService
{
    Task<GetTopicDetailResponse> GetTopicDetail(Guid topicId);
}
