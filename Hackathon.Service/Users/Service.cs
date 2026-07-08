using Hackathon.Repository;
using Hackathon.Repository.Entity;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Users;

public class Service : IService
{
    private readonly MediaService.IService _mediaService;  
    public readonly AppDbContext _dbContext;
    public readonly IHttpContextAccessor _IhttpContex;
    public Service(MediaService.IService mediaService, AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _mediaService = mediaService;  
        _dbContext = dbContext;
        _IhttpContex = httpContextAccessor;
    }

    
    public async Task<Reponse.UserDetailResponse> GetUserById(Guid userId)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId && !x.IsDisable);

        if (user == null) throw new NotFoundException("USER_NOT_FOUND");

        return new Reponse.UserDetailResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth,
            StudentId = user.StudentId,
            College = user.College,
            ImgUrl = user.ImgUrl,
            LinkUrl = user.LinkUrl,
            Role = user.Role,
            Status = user.Status,
            IsVerified = user.IsVerified
        };
    }

    public async Task<BasePaginationResponse> SearchStudents(Request.SearchStudentsRequest request)
    {
        var q = _dbContext.Users
            .AsNoTracking()
            .Where(x => x.Role == RoleEnum.Student && !x.IsDisable);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var normalized = request.Search.Trim().ToLower();
            q = q.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(normalized)
                || x.Email.ToLower().Contains(normalized));
        }

        var totalCount = await q.CountAsync();

        var pageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
        var pageSize = request.PageSize <= 0 ? 10 : System.Math.Min(request.PageSize, 100);

        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new Reponse.StudentSearchResponse
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                AvatarUrl = x.AvatarUrl,
                StudentId = x.StudentId,
                College = x.College,
                Status = x.Status
            })
            .ToListAsync();

        return ApiResponseFactory.BasePagination(items, pageIndex, pageSize, totalCount, _IhttpContex.HttpContext?.TraceIdentifier);
    }

    public async Task<Reponse.UserProfileDetailResponse> GetProfileUser()
    {
        var userId = GetUserId();
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        if(user == null) throw new NotFoundException("USER_NOT_FOUND");

        return new Reponse.UserProfileDetailResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth,
            StudentId = user.StudentId,
            College = user.College,
            ImgUrl = user.ImgUrl,
            LinkUrl = user.LinkUrl,
            Status = user.Status,
            BanReason = user.BanReason,
            Role = user.Role
        };
    }

    public async Task<string> UpdateProfile(Request.UpdateProfileRequest request)
    {
        var userId = GetUserId();
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new NotFoundException("USER_NOT_FOUND");

        if (request.FirstName != null) user.FirstName = request.FirstName;
        if (request.LastName != null) user.LastName = request.LastName;
        if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
        if (request.Bio != null) user.Bio = request.Bio;
        if (request.Address != null) user.Address = request.Address;
        if (request.DateOfBirth != null)
        {
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // +7 timezone for vn

            // Lấy UTC offset của múi giờ +7 (7 giờ)
            var offset = userTimeZone.GetUtcOffset(DateTime.UtcNow);

            // Khởi tạo DateTimeOffset tại đúng 0 giờ của ngày đó ở múi giờ +7, SAU ĐÓ chuyển về UTC để PostgreSQL có thể lưu được (Offset = 0)
            var localDob = new DateTimeOffset(request.DateOfBirth.Value.Year, request.DateOfBirth.Value.Month, request.DateOfBirth.Value.Day, 0, 0, 0, offset);
            user.DateOfBirth = localDob.ToUniversalTime();
        }
        if (request.StudentId != null) user.StudentId = request.StudentId;
        if (request.ImgUrl != null) user.ImgUrl = request.ImgUrl;
        if (request.LinkUrl != null) user.LinkUrl = request.LinkUrl;

        user.UpdatedAt = DateTimeOffset.UtcNow;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return "PROFILE_UPDATED_SUCCESSFULLY";
    }

    public async Task<string> CreateSystemReport(Request.CreateSystemReportRequest request)
    {
        var userId = GetUserId();

        var report = new Reports
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            TypeReport = request.TypeReport,
            Status = ReportStatusEnum.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _dbContext.Reports.Add(report);
        await _dbContext.SaveChangesAsync();

        return "REPORT_CREATED_SUCCESSFULLY";
    }

    public async Task<List<Reponse.MyAssignmentResponse>> GetMyAssignments()
    {
        var userId = GetUserId();
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId && !x.IsDisable);
        if (user == null) throw new NotFoundException("USER_NOT_FOUND");
        if (user.Role != RoleEnum.Staff && user.Role != RoleEnum.Lecturer) throw new ForbiddenException("FORBIDDEN");

        return await _dbContext.AssignEvents
            .AsNoTracking()
            .Where(x => x.UserId == userId && !x.IsDisable && !x.Event.IsDisable)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new Reponse.MyAssignmentResponse
            {
                AssignEventId = x.Id,
                EventId = x.EventId,
                EventName = x.Event.Name,
                Role = x.EventRole != null ? x.EventRole.Name : null,
                Tracks = x.AssignTracks
                    .Where(t => !t.IsDisable && !t.Track.IsDisable)
                    .Select(t => new Reponse.AssignmentTrackResponse
                    {
                        TrackId = t.TrackId,
                        TrackTitle = t.Track.Title
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<BasePaginationResponse> GetMyReports(Request.GetMyReportsRequest request)
    {
        var userId = GetUserId();
        var pageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
        var pageSize = request.PageSize <= 0 ? 10 : Math.Min(request.PageSize, 100);

        var query = _dbContext.Reports
            .AsNoTracking()
            .Where(x => x.UserId == userId && !x.IsDisable);

        var totalCount = await query.CountAsync();
        var reports = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new Reponse.MyReportListItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                TypeReport = x.TypeReport,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        return ApiResponseFactory.BasePagination(reports, pageIndex, pageSize, totalCount, _IhttpContex.HttpContext?.TraceIdentifier);
    }

    public async Task<Reponse.MyReportDetailResponse> GetMyReportById(Guid reportId)
    {
        var userId = GetUserId();
        var report = await _dbContext.Reports.AsNoTracking().FirstOrDefaultAsync(x => x.Id == reportId && !x.IsDisable);
        if (report == null) throw new NotFoundException("REPORT_NOT_FOUND", "Báo cáo không tồn tại.");
        if (report.UserId != userId) throw new ForbiddenException("FORBIDDEN");

        return new Reponse.MyReportDetailResponse
        {
            Id = report.Id,
            Title = report.Title,
            Description = report.Description,
            TypeReport = report.TypeReport,
            Status = report.Status,
            Reason = report.Reason,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt
        };
    }

    public async Task<string> UpdateAvatar(Request.UpdateAvatarRequest request)
    {
        if (request.AvatarUrl == null || request.AvatarUrl.Length == 0)
            throw new BadRequestException("AVATAR_FILE_REQUIRED");

        var userId = GetUserId();
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId && !x.IsDisable);
        if (user == null) throw new NotFoundException("USER_NOT_FOUND");

        user.AvatarUrl = await _mediaService.UploadImageAsync(request.AvatarUrl);
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync();
        return "AVATAR_UPDATED_SUCCESSFULLY";
    }

    private Guid GetUserId()
    {
        var userId = _IhttpContex?.HttpContext?.User.FindFirst("UserId")?.Value;
        if (!Guid.TryParse(userId, out var parsedUserId)) throw new UnauthorizedException("UNAUTHORIZED");
        return parsedUserId;
    }
}
