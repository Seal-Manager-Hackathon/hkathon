# Admin — Notification Mapping
> Controller: AdminRegisterTeamController / AdminAssignController / AdminReportController / AdminTeamController / AdminNotificationController

## Quy ước
- **Team** — toàn bộ thành viên trong team đều thấy; Admin/Staff có thể xem qua `GET /notifications?targetType=Team`.
- **Personal** — chỉ người được chỉ định `userId` thấy (Admin dùng `/notifications/my`).
- **System** — tất cả người dùng đều thấy.

## Register Team
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | register-teams/{id}/approve | ApproveRegisterTeam | Your team {TênTeam} has been approved for {TênEvent} | Team → teamId |
| POST | register-teams/{id}/reject | RejectRegisterTeam | Your team {TênTeam} has been rejected: {lý do} | Team → teamId |
| POST | register-teams/{id}/ban | BanRegisterTeam | Your team {TênTeam} has been banned: {lý do} | Team → teamId |
| POST | register-teams/{id}/unban | UnbanRegisterTeam | Your team {TênTeam} has been unbanned | Team → teamId |
| POST | register-teams/{id}/assign-next-round | AssignToNextRound | Your team {TênTeam} has advanced to round {RoundNo} | Team → teamId |
| POST | register-teams/{id}/revert-previous-round | RevertToPreviousRound | Your team {TênTeam} has been moved back to round {RoundNo} | Team → teamId |
| POST | register-teams/{id}/assign-track-topic | AssignTrackTopic | Your team {TênTeam} has been assigned track {TênTrack} for {TênEvent} + (nếu có topic) Your team {TênTeam} has been assigned topic {TênTopic} for {TênEvent} | Team → teamId |
| POST | register-teams/{id}/remove-track-topic | RemoveTrackTopic | Your team {TênTeam} has been removed from track {TênTrack} and topic {TênTopic} in {TênEvent} | Team → teamId |

## Assignment
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | assign/events/{eventId}/assign/users | AssignUserToEvent | You have been assigned as {role} for {event} | Personal → assigned user |
| PATCH | assign/event-assigns/{id}/event-role | AssignEventRoleToLecturer | Your role in {event} has been changed from {oldRole} to {newRole} | Personal → lecturer |
| POST | assign/event-assigns/{id}/tracks | AssignTrackToEvent | You have been assigned to track {track} in {event} | Personal → assigned user |
| POST | assign/event-assigns/{id}/tracks/{trackId}/remove | RemoveTrackFromEvent | You have been removed from track {track} in {event} | Personal → assigned user |
| POST | assign/event-assigns/{id}/tracks/{trackId}/restore | RestoreTrackToEvent | You have been restored to track {track} in {event} | Personal → assigned user |
| POST | assign/event-assigns/{id}/remove | RemoveAssignEvent | You have been removed from event {event} | Personal → assigned user |
| POST | assign/event-assigns/{id}/restore | RestoreAssignEvent | You have been restored to event {event} | Personal → assigned user |

## Report
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| PATCH | reports/{reportId}/status | UpdateReportStatus | Your report "{title}" has been {status} | Personal → reporter |

## Team
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | teams/{teamId}/change-leader | ChangeLeader | You are now the leader of team {TênTeam} | Personal → new leader |

## API Notification (đọc/sửa)
| Method | API | Service | Mục đích |
|--------|-----|---------|----------|
| GET | notifications/recent | GetRecentNotifications | Dashboard: 10 notification gần nhất (đã lọc disabled) |
| GET | notifications | GetNotifications | Danh sách quản lý toàn bộ notification, có lọc targetType |
| GET | notifications/{id} | GetNotificationDetail | Chi tiết (quản lý) |
| POST | notifications/{id}/delete | DeleteNotification | Disable notification |
| POST | notifications/{id}/restore | RestoreNotification | Khôi phục notification |
| PATCH | notifications/{id} | UpdateNotification | Sửa nội dung |
| POST | notifications | CreateNotification | Tạo thủ công (bởi Admin) |
| GET | notifications/my | GetMyNotifications | Inbox: Personal + System của Admin |
| GET | notifications/my/unread-count | GetMyUnreadCount | Đếm chưa đọc trong inbox |
| GET | notifications/my/{id} | GetMyNotificationDetail | Chi tiết inbox |
| POST | notifications/my/{id}/read | ReadNotification | Đánh dấu đã đọc |
| POST | notifications/my/read-all | ReadAllNotifications | Đánh dấu tất cả đã đọc |
