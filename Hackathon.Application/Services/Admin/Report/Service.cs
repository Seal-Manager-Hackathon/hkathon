using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.Report;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Report;

public class Service : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(IReportRepository reportRepository, IAuthorizationService authorizationService, IUnitOfWork unitOfWork)
    {
        _reportRepository = reportRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRecentReportsResponse> GetRecentReports()
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var reports = await _reportRepository.GetRecentAsync(10);

        return new GetRecentReportsResponse
        {
            Reports = reports.Select(r => new ReportItem
            {
                Id = r.Id,
                UserId = r.UserId,
                UserEmail = r.User.Email,
                UserFirstName = r.User.FirstName,
                UserLastName = r.User.LastName,
                Title = r.Title,
                Description = r.Description,
                Status = r.Status?.ToString(),
                TypeReport = r.TypeReport,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }

    public async Task<GetReportsResponse> GetReports(GetReportsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        ReportStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<ReportStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Reject, Resolved");
            status = parsed;
        }

        var (items, totalCount) = await _reportRepository.SearchAsync(
            request.Keyword, status,
            request.PageIndex, request.PageSize);

        return new GetReportsResponse
        {
            Items = items.Select(r => new ReportItem
            {
                Id = r.Id,
                UserId = r.UserId,
                UserEmail = r.User.Email,
                UserFirstName = r.User.FirstName,
                UserLastName = r.User.LastName,
                Title = r.Title,
                Description = r.Description,
                Status = r.Status?.ToString(),
                TypeReport = r.TypeReport,
                CreatedAt = r.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task UpdateReportStatus(Guid reportId, UpdateReportStatusRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        if (!Enum.TryParse<ReportStatusEnum>(request.Status, true, out var status))
            throw new BadRequestException("Invalid Status. Must be: Pending, Reject, Resolved");

        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        report.Status = status;
        report.Reason = request.Reason;
        report.UpdatedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetReportDetailResponse> GetReportDetail(Guid reportId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return new GetReportDetailResponse
        {
            Id = report.Id,
            UserId = report.UserId,
            UserEmail = report.User.Email,
            UserFirstName = report.User.FirstName,
            UserLastName = report.User.LastName,
            AssignEventId = report.AssignEventId,
            AssignEventUserName = report.AssignEvent?.User != null
                ? $"{report.AssignEvent.User.FirstName} {report.AssignEvent.User.LastName}"
                : null,
            SubmissionId = report.SubmissionId,
            Title = report.Title,
            Description = report.Description,
            ImgUrl = report.ImgUrl,
            FileUrl = report.FileUrl,
            Status = report.Status?.ToString(),
            Reason = report.Reason,
            TypeReport = report.TypeReport,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt
        };
    }
}
