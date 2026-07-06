using Hackathon.Repository;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Notifications;

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

    // #{All authenticated}
    public async Task<BasePaginationResponse> GetMyNotifications(PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<int> GetUnreadCount() => throw new NotImplementedException();
    public async Task<string> MarkAsRead(Guid notificationId) => throw new NotImplementedException();
    public async Task<string> MarkAllAsRead() => throw new NotImplementedException();
    public async Task<string> DisableAll() => throw new NotImplementedException();
}
