namespace Hackathon.Service.Topics;

public interface IService
{
    // #{Public} #{Student}
    Task<Response.AssignedTopicResponse> GetTopic(Guid eventId, Guid registerTeamId);
    Task<Response.TopicDetailResponse> GetTopicDetail(Guid topicId);

    // #{Admin} #{Staff}
    Task<Response.CreateTopicResponse> CreateTopic(Guid trackId, Request.CreateTopicRequest request);
    Task<string> UpdateTopic(Guid topicId, Request.UpdateTopicRequest request);
    Task<string> DeleteTopic(Guid topicId);
}
