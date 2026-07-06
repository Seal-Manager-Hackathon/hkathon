namespace Hackathon.Service.Notifications;

public static class NotificationTemplates
{
    public const string TeamInvitationAcceptedTitle = "Thành viên mới";
    public const string TeamInvitationAcceptedBody = "{0} đã chấp nhận lời mời vào team {1}.";

    public const string TeamInvitationRejectedTitle = "Lời mời bị từ chối";
    public const string TeamInvitationRejectedBody = "{0} đã từ chối lời mời vào team {1}.";

    public const string TeamInvitationReceivedTitle = "Lời mời vào team";
    public const string TeamInvitationReceivedBody = "Bạn đã được mời vào team {0}. Bạn có muốn tham gia không?";

    public const string RegisterApprovedTitle = "Đăng ký được duyệt";
    public const string RegisterApprovedBody = "Team {0} đã được duyệt tham gia event {1}.";

    public const string RegisterRejectedTitle = "Đăng ký bị từ chối";
    public const string RegisterRejectedBody = "Team {0} đã bị từ chối tham gia event {1}. Lý do: {2}.";

    public const string ScorePublishedTitle = "Điểm đã được công bố";
    public const string ScorePublishedBody = "Điểm round {0} của event {1} đã được công bố.";

    public const string TeamMemberRemovedTitle = "Rời team";
    public const string TeamMemberRemovedBody = "{0} đã bị xóa khỏi team {1}.";

    public const string AppealSubmittedTitle = "Khiếu nại điểm";
    public const string AppealSubmittedBody = "Team {0} đã gửi khiếu nại cho round {1}.";
}
