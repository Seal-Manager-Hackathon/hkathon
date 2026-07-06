using Microsoft.AspNetCore.Http;

namespace Hackathon.Service.MediaService;

public interface IService
{
    public Task<string> UploadImageAsync(IFormFile file);
}