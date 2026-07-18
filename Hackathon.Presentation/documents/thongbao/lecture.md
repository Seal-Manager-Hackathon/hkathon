# Lecturer — Notification Mapping
> Controller: JudgeController / MentorNotificationController / LecturerAssignController

## Judge — Scoring
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | submissions/{submissionId}/scores | SubmitScore | Your submission for round {RoundNo} has been scored | Personal → all team members |

## Mentor — Mentor Notification
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | mentors/tracks/{trackId}/notifications | SendTrackNotification | *(Dùng MentorNotifications entity riêng)* | Team → teams in track |
| POST | mentors/teams/{teamId}/notifications | SendTeamNotification | *(Dùng MentorNotifications entity riêng)* | Personal → team members |
