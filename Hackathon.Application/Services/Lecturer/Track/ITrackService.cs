using Hackathon.Application.Services.Admin.Track;

namespace Hackathon.Application.Services.Lecturer.Track;

public interface ITrackService
{
    Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId);
}
