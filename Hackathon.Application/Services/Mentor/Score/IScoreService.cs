using Hackathon.Application.Services.Admin.Score;

namespace Hackathon.Application.Services.Mentor.Score;

public interface IScoreService
{
    Task<GetTeamRoundScoreResponse> GetTeamRoundScore(Guid roundId, Guid registerTeamId);
    Task<GetRegisterTeamScoresResponse> GetRegisterTeamScores(Guid registerTeamId);
}
