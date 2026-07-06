using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Invitations;

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

    // #{Student}
    public async Task<BasePaginationResponse> GetMyInvitations(PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.InvitationItemResponse> AcceptInvitation(Guid invitationId) => throw new NotImplementedException();
    public async Task<Response.InvitationItemResponse> RejectInvitation(Guid invitationId) => throw new NotImplementedException();
    public async Task<int> GetPendingInvitationsCount() => throw new NotImplementedException();
}
