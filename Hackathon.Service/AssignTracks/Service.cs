using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.AssignTracks;

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
    public async Task<Response.AssignTrackResponse> AssignLecturerToTrack(Guid eventId, Guid trackId, Request.AssignJudgeRequest request) => throw new NotImplementedException();
    public async Task<List<Response.AssignTrackLecturerResponse>> GetLecturersAssignedToTrack(Guid eventId, Guid trackId, bool? isDisable) => throw new NotImplementedException();
    public async Task<Guid> RemoveLecturerFromTrack(Guid assignTrackId) => throw new NotImplementedException();
}
