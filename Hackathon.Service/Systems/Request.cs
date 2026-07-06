using Microsoft.AspNetCore.Http;

namespace Hackathon.Service.Systems;

public static class Request
{
    public class FileUploadRequest
    {
        public IFormFile? File { get; set; }
        public string? Folder { get; set; }
    }
}
