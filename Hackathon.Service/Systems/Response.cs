namespace Hackathon.Service.Systems;

public static class Response
{
    public class HealthResponse
    {
        public int Status { get; set; }
        public int Database { get; set; }
        public int UptimeSeconds { get; set; }
    }

    public class VersionResponse
    {
        public string Version { get; set; } = null!;
        public string Environment { get; set; } = null!;
        public string DotnetVersion { get; set; } = null!;
    }

    public class UploadFileResponse
    {
        public string FileUrl { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public long FileSize { get; set; }
    }
}
