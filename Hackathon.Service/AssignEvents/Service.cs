using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.AssignEvents;

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
    private async Task EnsureStaffAssignedToEvent(Guid eventId) => throw new NotImplementedException();

    // #{Staff} #{Admin}
    public async Task<Response.AssignEventResponse> AssignLecturerToEvent(Guid eventId, Request.AssignLecturerRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetAvailableLecturers(Guid eventId, Request.GetAvailableLecturersRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetEventAssignments(Guid eventId, EventRoleEnum? eventRole, string? keyword, Guid? trackId, bool? isDisable, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Guid> RemoveLecturerAssignment(Guid assignEventId) => throw new NotImplementedException();
}
