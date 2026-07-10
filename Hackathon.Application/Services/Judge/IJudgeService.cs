namespace Hackathon.Application.Services.Judge;

public interface IJudgeService
{
    Task<List<JudgeTrackItem>> GetMyTracks(Guid eventId);
    Task<GetTrackSubmissionsResponse> GetTrackSubmissions(Guid trackId, Guid? roundId, int pageIndex, int pageSize);
}
