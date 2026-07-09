using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Helper xác thực Staff được phân công vào event.
/// Kiểm tra:
/// - Staff có trong AssignEvents không
/// - Event có đang trong thời gian hoạt động (tuỳ chọn)
/// - Trả về thông tin assign + event để controller/service dùng tiếp
/// </summary>
public static class StaffAssignmentHelper
{
    /// <summary>
    /// Kiểm tra staff có assign vào event không.
    /// Nếu có, trả về AssignEvents (kèm Event, AssignTracks).
    /// </summary>
    public static async Task<AssignEvents> ValidateAndGetAssignmentAsync(
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        Guid eventId,
        bool checkEventTime = false)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await assignEventRepository.GetByEventIdAndUserIdWithEventAsync(
            eventId, currentUserId);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        if (checkEventTime && assignEvent.Event != null)
        {
            var now = DateTimeOffset.UtcNow;
            if (assignEvent.Event.StartTime.HasValue && now < assignEvent.Event.StartTime.Value)
                throw new BadRequestException("Event Has Not Started Yet");
            if (assignEvent.Event.EndTime.HasValue && now > assignEvent.Event.EndTime.Value)
                throw new BadRequestException("Event Has Ended");
        }

        return assignEvent;
    }

    /// <summary>
    /// Kiểm tra staff có thể tác động lên register team không.
    /// Load register team → lấy EventId → gọi ValidateAndGetAssignmentAsync.
    /// </summary>
    public static async Task<AssignEvents> ValidateAssignmentForRegisterTeamAsync(
        IRegisterTeamRepository registerTeamRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        Guid registerTeamId,
        bool checkEventTime = false)
    {
        var rt = await registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null)
            throw new NotFoundException("Register Team Not Found");

        return await ValidateAndGetAssignmentAsync(
            assignEventRepository, currentUserService, rt.EventId, checkEventTime);
    }
}
