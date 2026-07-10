namespace Hackathon.Application.Services.Lecturer.Event;

public interface IEventService
{
    Task<GetLecturerEventsResponse> GetLecturerEvents(GetLecturerEventsRequest request);
    Task<GetLecturerEventDetailResponse> GetLecturerEventDetail(Guid eventId);
    Task<GetLecturerAssignedInfoResponse> GetLecturerAssignedInfo(Guid eventId);
}
