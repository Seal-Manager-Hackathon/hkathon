using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Staff;

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

    // #{Staff}
    public async Task<List<Response.StaffEventResponse>> GetCurrentStaffEvents() => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetStaffEvents(PaginationRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> SearchStaffEvents(Request.SearchStaffEventsRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetReports(Request.GetStaffReportsRequest request) => throw new NotImplementedException();
    public async Task<Response.StaffReportDetailResponse> GetReportDetail(Guid reportId) => throw new NotImplementedException();
    public async Task<Response.ApproveRegradeResponse> ApproveRegrade(Guid reportId) => throw new NotImplementedException();
    public async Task UpdateReportStatus(Guid reportId, Request.UpdateReportStatusRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetRegradeSubmissions(Request.GetRegradeSubmissionsRequest request) => throw new NotImplementedException();
    public async Task<string> ChangeUserRole(Guid userId, Request.StaffChangeUserRoleRequest request) => throw new NotImplementedException();
}
