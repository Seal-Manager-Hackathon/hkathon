using Hackathon.Service.Models;

namespace Hackathon.Service.Mentors;

public interface IService
{
    // #{Mentor}
    Task<BasePaginationResponse> GetMentorEvents(Request.GetMentorEventsRequest request);
    Task<List<Response.MentorTrackResponse>> GetMentorTracks(Guid? eventId);
    Task<BasePaginationResponse> GetMentorTrackTeams(Guid trackId, PaginationRequest paginationRequest);
    Task<Response.MentorNotificationResponse> SendTrackNotification(Guid trackId, Request.SendNotificationRequest request);
    Task<Response.MentorNotificationResponse> SendTeamNotification(Guid teamId, Guid? trackId, Request.SendNotificationRequest request);
}
