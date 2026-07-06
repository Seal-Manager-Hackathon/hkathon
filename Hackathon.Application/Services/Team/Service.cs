using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.Team;

public class Service : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(ITeamRepository teamRepository, IAuthorizationService authorizationService)
    {
        _teamRepository = teamRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var total = await _teamRepository.CountAsync(request.IsDisable);

        return new GetTeamCountResponse
        {
            Total = total
        };
    }
}
