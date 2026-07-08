using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;

namespace Hackathon.Service.Users;

public static class Request
{
    public class UpdateProfileRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? StudentId { get; set; }
        public string? ImgUrl { get; set; }
        public string? LinkUrl { get; set; }

    }

    public class CreateSystemReportRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? ImgUrl { get; set; }
        public string? TypeReport { get; set; }
    }

    public class GetMyReportsRequest : PaginationRequest
    {
    }

    public class SearchStudentsRequest : PaginationRequest
    {
        public string? Search { get; set; }
    }

    public class UpdateAvatarRequest
    {
        public IFormFile? AvatarUrl { get; set; }
    }
}
