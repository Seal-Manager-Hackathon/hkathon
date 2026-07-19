# Lecturer — Notification Mapping
> Controller: LecturerNotificationController / LecturerScoreController / LecturerAssignController

## Nhận thông báo
- **Personal** — assignment từ Admin (event, track, role), report status
- **Team** — không nhận trực tiếp (lecturer không phải member của team)

## Đọc notification
| Method | API | Service | Mục đích |
|--------|-----|---------|----------|
| GET | notifications/my | GetMyNotifications | Inbox: Personal + System |
| GET | notifications/my/unread-count | GetMyUnreadCount | Đếm chưa đọc |
| GET | notifications/my/{id} | GetMyNotificationDetail | Chi tiết inbox |
| POST | notifications/my/{id}/read | ReadMyNotification | Đánh dấu đã đọc |
| POST | notifications/my/read-all | ReadAllMyNotifications | Đánh dấu tất cả đã đọc |

> **Ghi chú:** Lecturer không tạo notification. Chỉ nhận từ Admin/Staff.
> Lecturer có thể mang role Judge (chấm điểm) — việc chấm điểm **không tạo notification**.
> Lecturer có thể mang role Mentor (gửi thông báo) — dùng entity `MentorNotifications` riêng (xem mentor.md).
