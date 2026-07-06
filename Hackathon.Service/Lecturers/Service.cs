using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Lecturers;

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

    // #{Lecturer}
    public async Task<BasePaginationResponse> GetLecturerEvents(PaginationRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> SearchLecturerEvents(Request.SearchLecturerEventsRequest request) => throw new NotImplementedException();
    public async Task<List<Response.LecturerEventResponse>> GetCurrentLecturerEvents() => throw new NotImplementedException();
    public async Task<Response.LecturerEventTracksResponse> GetLecturerTracks(Guid eventId) => throw new NotImplementedException();
}
