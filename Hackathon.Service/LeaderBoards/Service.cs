using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.LeaderBoards;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;

    public Service(AppDbContext dbContext, IHttpContextAccessor httpContext)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
    }

    private Guid GetCurrentUserId() => throw new NotImplementedException();
    private bool IsCurrentUserAdmin() => throw new NotImplementedException();

    // #{Public}
    public async Task<List<Response.YearLeaderboardResponse>> GetYearLeaderboard(int year) => throw new NotImplementedException();

    // #{Staff} #{Admin}
    public async Task<string> AssignAward(Guid leaderBoardId, Guid teamId, Request.AssignAwardRequest request) => throw new NotImplementedException();
}
