using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Admin;

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

    // #{Admin}
    public async Task<BasePaginationResponse> GetAllUsers(RoleEnum? role, string? keyword, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> SearchUsers(GetUsersQuery query) => throw new NotImplementedException();
    public async Task<string> ChangeUserRole(Guid userId, ChangeUserRoleRequest request) => throw new NotImplementedException();
    public async Task<SendSystemNotificationResponse> SendSystemNotification(SendSystemNotificationRequest request) => throw new NotImplementedException();

    // #{Admin} — Rounds
    public async Task<BasePaginationResponse> GetRounds(Guid eventId, GetAdminRoundsRequest request) => throw new NotImplementedException();
    public async Task<CreateRoundResponse> CreateRound(Guid eventId, CreateRoundRequest request) => throw new NotImplementedException();
    public async Task UpdateRound(Guid roundId, UpdateRoundRequest request) => throw new NotImplementedException();
    public async Task DeleteRound(Guid roundId) => throw new NotImplementedException();
    public async Task<string> RestoreRound(Guid roundId) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetRoundSubmissions(Guid roundId, Rounds.Request.GetStaffRoundSubmissionsQuery query) => throw new NotImplementedException();
}
