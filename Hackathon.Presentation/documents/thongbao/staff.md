# Staff — Notification Mapping
> Controller: StaffRegisterTeamController / StaffReportController / StaffAssignController / StaffNotificationController

## Quy ước
- **Team** — toàn bộ thành viên trong team đều thấy; Staff có thể xem qua `GET /notifications?targetType=Team`.
- **Personal** — chỉ người được chỉ định `userId` thấy.
- **System** — tất cả người dùng đều thấy.

## Register Team
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | register-teams/{registerTeamId}/approve | ApproveRegisterTeam | Your team {TênTeam} has been approved for {TênEvent} | Team → teamId |
| POST | register-teams/{registerTeamId}/reject | RejectRegisterTeam | Your team {TênTeam} has been rejected: {lý do} | Team → teamId |
| POST | register-teams/{registerTeamId}/ban | BanRegisterTeam | Your team {TênTeam} has been banned: {lý do} | Team → teamId |
| POST | register-teams/{registerTeamId}/unban | UnbanRegisterTeam | Your team {TênTeam} has been unbanned | Team → teamId |
| POST | register-teams/{registerTeamId}/assign-track-topic | AssignTrackTopic | Your team {TênTeam} has been assigned track {TênTrack} for {TênEvent} + (nếu có topic) Your team {TênTeam} has been assigned topic {TênTopic} for {TênEvent} | Team → teamId |
| POST | register-teams/{registerTeamId}/remove-track-topic | RemoveTrackTopic | Your team {TênTeam} has been removed from track {TênTrack} and topic {TênTopic} in {TênEvent} | Team → teamId |
| POST | register-teams/{registerTeamId}/assign-next-round | AssignToNextRound | Your team {TênTeam} has advanced to round {RoundNo} | Team → teamId |
| POST | register-teams/{registerTeamId}/revert-previous-round | RevertToPreviousRound | Your team {TênTeam} has been moved back to round {RoundNo} | Team → teamId |

## Report
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| PATCH | reports/{reportId}/status | UpdateReportStatus | Your report "{title}" has been {status} | Personal → reporter |

## Assign Lecturer
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | events/{eventId}/assign/users | AssignLecturerToEvent | You have been assigned as {role} for {event} | Personal → lecturer |
| POST | event-assigns/{assignEventId}/remove | RemoveAssignEvent | You have been removed from event {event} | Personal → lecturer |
| POST | event-assigns/{assignEventId}/restore | RestoreAssignEvent | You have been restored to event {event} | Personal → lecturer |
| POST | event-assigns/{assignEventId}/tracks | AssignTrackToEvent | You have been assigned to track {track} in {event} | Personal → lecturer |
| POST | event-assigns/{assignEventId}/tracks/{trackId}/remove | RemoveTrackFromEvent | You have been removed from track {track} in {event} | Personal → lecturer |
| POST | event-assigns/{assignEventId}/tracks/{trackId}/restore | RestoreTrackToEvent | You have been restored to track {track} in {event} | Personal → lecturer |

## API Notification
| Method | API | Service | Mục đích |
|--------|-----|---------|----------|
| GET | notifications/recent | GetRecentNotifications | Dashboard: 10 notification gần nhất |
| GET | notifications | GetNotifications | Danh sách quản lý toàn bộ notification |
| GET | notifications/{id} | GetNotificationDetail | Chi tiết |
| POST | notifications/{id}/delete | DeleteNotification | Disable notification |
| POST | notifications/{id}/restore | RestoreNotification | Khôi phục notification |
| PATCH | notifications/{id} | UpdateNotification | Sửa nội dung |
| POST | notifications | CreateNotification | Tạo thủ công |
| GET | notifications/my | GetMyNotifications | Inbox: Personal + System + Team của Staff |
| GET | notifications/my/unread-count | GetMyUnreadCount | Đếm chưa đọc trong inbox |
| GET | notifications/my/{id} | GetMyNotificationDetail | Chi tiết inbox |
| POST | notifications/my/{id}/read | ReadMyNotification | Đánh dấu đã đọc |
| POST | notifications/my/read-all | ReadAllMyNotifications | Đánh dấu tất cả đã đọc |
