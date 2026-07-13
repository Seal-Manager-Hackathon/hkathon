using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Report;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Report;

public class Service : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IReportRepository reportRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _reportRepository = reportRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateReportResponse> CreateReport(CreateReportRequest request)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var now = DateTimeOffset.UtcNow;
        var report = new Reports
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            TypeReport = request.TypeReport,
            Status = ReportStatusEnum.Pending,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _reportRepository.AddAsync(report);
        await _unitOfWork.SaveChangesAsync();

        return new CreateReportResponse
        {
            Id = report.Id,
            Title = report.Title,
            Status = report.Status?.ToString(),
            CreatedAt = report.CreatedAt
        };
    }

    public async Task<GetReportsResponse> GetReports(GetReportsRequest request)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        ReportStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<ReportStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Reject, Resolved, Canceled");
            status = parsed;
        }

        var (items, totalCount) = await _reportRepository.GetByUserIdAsync(
            userId, request.Keyword, status,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetReportsResponse
        {
            Items = items.Select(r => new ReportItem
            {
                Id = r.Id,
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

    public async Task<GetReportDetailResponse> GetReportDetail(Guid reportId)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Student only sees their own reports
        if (report.UserId != userId)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return new GetReportDetailResponse
        {
            Id = report.Id,
            UserId = report.UserId,
            Title = report.Title,
            Description = report.Description,
            Status = report.Status?.ToString(),
            Reason = report.Reason,
            TypeReport = report.TypeReport,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt
        };
    }

    public async Task CancelReport(Guid reportId)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Student only cancels their own reports
        if (report.UserId != userId)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Can only cancel Pending reports
        if (report.Status != ReportStatusEnum.Pending)
            throw new BadRequestException("Cannot Cancel a Report That Has Been Processed");

        report.Status = ReportStatusEnum.Canceled;
        report.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }
}
