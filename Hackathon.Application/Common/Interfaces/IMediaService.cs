using Microsoft.AspNetCore.Http;

namespace Hackathon.Application.Common.Interfaces;

public interface IMediaService
{
    Task<string> UploadImageAsync(IFormFile file, string folder);
}