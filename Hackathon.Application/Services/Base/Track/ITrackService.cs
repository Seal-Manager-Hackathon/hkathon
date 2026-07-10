using Hackathon.Application.Services.Admin.Track;

namespace Hackathon.Application.Services.Base.Track;

public interface ITrackService
{
    Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId);
}
