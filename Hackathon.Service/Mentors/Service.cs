using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Mentors;

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

    // #{Mentor}
    public async Task<BasePaginationResponse> GetMentorEvents(Request.GetMentorEventsRequest request) => throw new NotImplementedException();
    public async Task<List<Response.MentorTrackResponse>> GetMentorTracks(Guid? eventId) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetMentorTrackTeams(Guid trackId, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.MentorNotificationResponse> SendTrackNotification(Guid trackId, Request.SendNotificationRequest request) => throw new NotImplementedException();
    public async Task<Response.MentorNotificationResponse> SendTeamNotification(Guid teamId, Guid? trackId, Request.SendNotificationRequest request) => throw new NotImplementedException();
}
