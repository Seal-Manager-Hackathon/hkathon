namespace Hackathon.Service.Systems;

public interface IService
{
    // #{Public}
    Dictionary<string, Dictionary<string, string>> GetEnums();
    Task<Response.HealthResponse> GetHealth(DateTime startupTime);
    Response.VersionResponse GetVersion(string environmentName);

    // #{All authenticated}
    Task<Response.UploadFileResponse> UploadFile(Request.FileUploadRequest request);
}
