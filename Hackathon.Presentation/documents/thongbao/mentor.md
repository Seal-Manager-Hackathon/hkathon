# Mentor — Notification Mapping
> Controller: MentorNotificationController

## Mentor Notification (gửi cho team)
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | mentors/tracks/{trackId}/notifications | SendTrackNotification | *(Dùng MentorNotifications entity riêng)* | Team → teams in track |
| POST | mentors/teams/{teamId}/notifications | SendTeamNotification | *(Dùng MentorNotifications entity riêng)* | Team → team members |

> **Note:** MentorNotifications là entity riêng (khác Notifications thường). Student đọc qua StudentNotificationController.
