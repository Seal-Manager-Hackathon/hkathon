using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.Admin;

public interface IService
{
    // #{Admin}
    Task<BasePaginationResponse> GetAllUsers(RoleEnum? role, string? keyword, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> SearchUsers(GetUsersQuery query);
    Task<string> ChangeUserRole(Guid userId, ChangeUserRoleRequest request);
    Task<SendSystemNotificationResponse> SendSystemNotification(SendSystemNotificationRequest request);

    // #{Admin} — Rounds
    Task<BasePaginationResponse> GetRounds(Guid eventId, GetAdminRoundsRequest request);
    Task<CreateRoundResponse> CreateRound(Guid eventId, CreateRoundRequest request);
    Task UpdateRound(Guid roundId, UpdateRoundRequest request);
    Task DeleteRound(Guid roundId);
    Task<string> RestoreRound(Guid roundId);
    Task<BasePaginationResponse> GetRoundSubmissions(Guid roundId, Rounds.Request.GetStaffRoundSubmissionsQuery query);
}
