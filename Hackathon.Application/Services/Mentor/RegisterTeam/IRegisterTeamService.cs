namespace Hackathon.Application.Services.Mentor.RegisterTeam;

public interface IRegisterTeamService
{
    Task<GetRegisterTeamByTrackResponse> GetRegisterTeamsByTrack(Guid trackId, string? keyword, int pageIndex, int pageSize);
}
