using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.RegisterTeam;

public class Service : IRegisterTeamRoundService
{
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRegisterTeamRepository registerTeamRepository,
        IAuthorizationService authorizationService)
    {
        _registerTeamRepository = registerTeamRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetRegisterTeamRoundResponse> GetCurrentRound(Guid registerTeamId)
    {
        // Chỉ cần đăng nhập là dùng được
        _authorizationService.Authenticate();

        var rt = await _registerTeamRepository.GetByIdWithRoundDetailsAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        // Lấy round detail gần nhất (có thể là round hiện tại)
        var currentRoundDetail = rt.RoundDetails
            .OrderByDescending(rd => rd.Round?.RoundNo)
            .FirstOrDefault();

        return new GetRegisterTeamRoundResponse
        {
            RegisterTeamId = rt.Id,
            TeamId = rt.TeamId,
            TeamName = rt.Team?.Name,
            TrackId = rt.TrackId,
            TrackName = rt.Track?.Title,
            TopicId = rt.TopicId,
            TopicName = rt.Topic?.Title,
            CurrentRoundId = currentRoundDetail?.RoundId,
            CurrentRoundName = currentRoundDetail?.Round?.Name,
            CurrentRoundNo = currentRoundDetail?.Round?.RoundNo
        };
    }
}
