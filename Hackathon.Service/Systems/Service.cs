using Hackathon.Repository;
using Microsoft.AspNetCore.Http;

namespace Hackathon.Service.Systems;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;

    public Service(AppDbContext dbContext, IHttpContextAccessor httpContext)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
    }

    // #{Public}
    public Dictionary<string, Dictionary<string, string>> GetEnums() => throw new NotImplementedException();
    public async Task<Response.HealthResponse> GetHealth(DateTime startupTime) => throw new NotImplementedException();
    public Response.VersionResponse GetVersion(string environmentName) => throw new NotImplementedException();

    // #{All authenticated}
    public async Task<Response.UploadFileResponse> UploadFile(Request.FileUploadRequest request) => throw new NotImplementedException();
}
