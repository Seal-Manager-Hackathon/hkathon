using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.RegisterTeam;

public class Service : IRegisterTeamService
{
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IRegisterTeamRepository registerTeamRepository,
        ITeamRepository teamRepository,
        IRoundRepository roundRepository,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _registerTeamRepository = registerTeamRepository;
        _teamRepository = teamRepository;
        _roundRepository = roundRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRegisterTeamsResponse> GetRegisterTeams(GetRegisterTeamsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RegisterTeamStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected");
            status = parsed;
        }

        var (items, totalCount) = await _registerTeamRepository.SearchAsync(
            request.EventId, request.Keyword, status,
            request.IsBanned, request.IsDisable,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetRegisterTeamsResponse
        {
            RegisterTeams = items.Select(rt => new RegisterTeamCard
            {
                Id = rt.Id,
                TeamId = rt.TeamId,
                TeamName = rt.Team?.Name,
                EventId = rt.EventId,
                EventName = rt.Event?.Name,
                TrackId = rt.TrackId,
                TrackName = rt.Track?.Title,
                TopicId = rt.TopicId,
                TopicName = rt.Topic?.Title,
                Description = rt.Description,
                RejectionReason = rt.RejectionReason,
                Status = rt.Status?.ToString(),
                IsBanned = rt.IsBanned,
                IsDisable = rt.IsDisable,
                CreatedAt = rt.CreatedAt,
                UpdatedAt = rt.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        var members = await _teamRepository.GetTeamMembersAsync(rt.TeamId);

        return new RegisterTeamDetailResponse
        {
            // RegisterTeam
            Id = rt.Id,
            Description = rt.Description,
            RejectionReason = rt.RejectionReason,
            Status = rt.Status?.ToString(),
            IsBanned = rt.IsBanned,
            IsDisable = rt.IsDisable,
            CreatedAt = rt.CreatedAt,
            UpdatedAt = rt.UpdatedAt,

            // Event
            EventId = rt.EventId,
            EventName = rt.Event?.Name,
            EventDescription = rt.Event?.Description,
            EventStartDate = rt.Event?.StartTime,
            EventEndDate = rt.Event?.EndTime,

            // Team
            TeamId = rt.TeamId,
            TeamName = rt.Team?.Name,
            TeamCanEdit = rt.Team?.CanEdit ?? false,
            TeamIsDisable = rt.Team?.IsDisable ?? false,
            TeamCreatedAt = rt.Team?.CreatedAt ?? default,

            // Track / Topic
            TrackId = rt.TrackId,
            TrackTitle = rt.Track?.Title,
            TopicId = rt.TopicId,
            TopicTitle = rt.Topic?.Title,

            // Members
            Members = members.Select(m => new RegisterTeamMemberItem
            {
                UserId = m.UserId,
                Email = m.User?.Email ?? "",
                FirstName = m.User?.FirstName ?? "",
                LastName = m.User?.LastName ?? "",
                AvatarUrl = m.User?.AvatarUrl,
                IsLeader = m.IsLeader,
                Status = m.Status?.ToString()
            }).ToList()
        };
    }

    public async Task UpdateRegisterTeam(UpdateRegisterTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(request.RegisterTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (request.Description != null)
            rt.Description = request.Description;
        if (request.RejectionReason != null)
            rt.RejectionReason = request.RejectionReason;
        if (request.Status != null)
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var status))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected");
            rt.Status = status;
        }
        if (request.IsBanned.HasValue)
            rt.IsBanned = request.IsBanned.Value;
        if (request.IsDisable.HasValue)
            rt.IsDisable = request.IsDisable.Value;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ApproveRegisterTeam(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.Status != RegisterTeamStatusEnum.Pending)
            throw new BadRequestException("Only Pending Register Team Can Be Approved");

        // Check round No1 của event
        var firstRound = await _roundRepository.GetFirstRoundByEventIdAsync(rt.EventId);
        if (firstRound != null && firstRound.LimitTeam.HasValue)
        {
            var currentTeamCount = await _roundRepository.CountTeamsInRoundAsync(firstRound.Id);
            if (currentTeamCount >= firstRound.LimitTeam.Value)
                throw new BadRequestException("Round 1 Is Full. Cannot Approve More Teams");
        }

        rt.Status = RegisterTeamStatusEnum.Approved;

        // Khóa team: không thể chỉnh sửa thành viên
        var team = await _teamRepository.GetByIdAsync(rt.TeamId);
        if (team != null)
        {
            team.CanEdit = false;
            await _teamRepository.UpdateAsync(team);
        }

        // Tự động thêm team vào round đầu tiên nếu có
        if (firstRound != null)
        {
            var roundDetail = new RoundDetails
            {
                Id = Guid.NewGuid(),
                RoundId = firstRound.Id,
                RegisterTeamId = registerTeamId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _roundRepository.AddRoundDetailAsync(roundDetail);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(GetRegisterTeamsByTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RegisterTeamStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<RegisterTeamStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Approved, Rejected");
            status = parsed;
        }

        var (items, totalCount) = await _registerTeamRepository.GetByTeamIdAsync(
            request.TeamId, status, request.PageIndex, request.PageSize);

        return new GetRegisterTeamsResponse
        {
            RegisterTeams = items.Select(rt => new RegisterTeamCard
            {
                Id = rt.Id,
                TeamId = rt.TeamId,
                TeamName = rt.Team?.Name,
                EventId = rt.EventId,
                EventName = rt.Event?.Name,
                TrackId = rt.TrackId,
                TrackName = rt.Track?.Title,
                TopicId = rt.TopicId,
                TopicName = rt.Topic?.Title,
                Description = rt.Description,
                RejectionReason = rt.RejectionReason,
                Status = rt.Status?.ToString(),
                IsBanned = rt.IsBanned,
                IsDisable = rt.IsDisable,
                CreatedAt = rt.CreatedAt,
                UpdatedAt = rt.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetUserEventsResponse> GetUserEvents(GetUserEventsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _registerTeamRepository.GetApprovedByUserIdAsync(
            request.UserId, request.Keyword, request.PageIndex, request.PageSize);

        return new GetUserEventsResponse
        {
            Events = items.Select(rt => new UserEventItem
            {
                RegisterTeamId = rt.Id,
                Status = rt.Status?.ToString(),
                IsBanned = rt.IsBanned,
                IsDisable = rt.IsDisable,
                CreatedAt = rt.CreatedAt,
                UpdatedAt = rt.UpdatedAt,

                // Event
                EventId = rt.EventId,
                EventName = rt.Event?.Name,
                EventDescription = rt.Event?.Description,
                EventStartTime = rt.Event?.StartTime,
                EventEndTime = rt.Event?.EndTime,
                EventStatus = rt.Event?.Status?.ToString(),

                // Team
                TeamId = rt.TeamId,
                TeamName = rt.Team?.Name,

                // Track / Topic
                TrackId = rt.TrackId,
                TrackTitle = rt.Track?.Title,
                TopicId = rt.TopicId,
                TopicTitle = rt.Topic?.Title
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task RejectRegisterTeam(Guid registerTeamId, string? rejectionReason)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.Status != RegisterTeamStatusEnum.Pending)
            throw new BadRequestException("Only Pending Register Team Can Be Rejected");

        rt.Status = RegisterTeamStatusEnum.Rejected;
        if (rejectionReason != null)
            rt.RejectionReason = rejectionReason;

        // Kiểm tra team có register team khác đã approved không
        var hasOtherApproved = await _registerTeamRepository.HasOtherApprovedAsync(rt.TeamId, registerTeamId);

        var team = await _teamRepository.GetByIdAsync(rt.TeamId);
        if (team != null && !hasOtherApproved)
        {
            // Không còn register team nào approved → mở khóa team
            team.CanEdit = true;
            await _teamRepository.UpdateAsync(team);
        }
        // Nếu còn register team khác approved → CanEdit vẫn false (đã khóa)

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task BanRegisterTeam(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        if (rt.IsBanned)
            throw new BadRequestException("Register Team Is Already Banned");

        rt.IsBanned = true;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UnbanRegisterTeam(Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        rt.IsBanned = false;
        await _unitOfWork.SaveChangesAsync();
    }
}
