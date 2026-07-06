using System;
using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.Staff;

public static class Request
{
    public class SearchStaffEventsRequest : PaginationRequest
    {
        public string? Keyword { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public EventStatusEnum? Status { get; set; }
    }

    public class GetStaffReportsRequest : PaginationRequest
    {
        public ReportStatusEnum? Status { get; set; }
        public string? TypeReport { get; set; }
        public Guid? EventId { get; set; }
        public string? Keyword { get; set; }
    }

    public class UpdateReportStatusRequest
    {
        public ReportStatusEnum Status { get; set; }
        public string? Reason { get; set; }
    }

    public class GetRegradeSubmissionsRequest : PaginationRequest
    {
        public Guid? EventId { get; set; }
        public Guid? TrackId { get; set; }
        public string? RegradeStatus { get; set; }
    }

    public class StaffChangeUserRoleRequest
    {
        public RoleEnum Role { get; set; }
    }
}
