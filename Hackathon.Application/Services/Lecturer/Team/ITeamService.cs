using Hackathon.Application.Services.Admin.Team;

namespace Hackathon.Application.Services.Lecturer.Team;

public interface ITeamService
{
    Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId);
}
