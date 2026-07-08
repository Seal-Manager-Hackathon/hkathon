# Main Features

## Account & Auth
- Register/login/logout.
- Refresh token.
- Reset password.
- Email verification.
- User status/ban handling.

## User Profile
- View profile (`GET /api/v1/user/me`).
- Update profile (`PATCH /api/v1/user/me`) — fields: firstName, lastName, phoneNumber, bio, address, dateOfBirth, studentId (set once only), imgUrl, linkUrl. Cannot change AvatarUrl or College.
- Validate required student profile fields.
- Store profile fields in `Users`.

## Role & Assignment
- Manage global role through `Users.Role` (`RoleEnum`); there are no `Roles`/`UserRoles` tables.
- Assign staff/lecture to event via `AssignEvents`.
- Assign mentor/judge to track via `AssignTracks`.

## Event Management
- Create/update event.
- Configure registration time, team limits, member limits, status, season.
- Configure awards and leaderboard.

## Round & Criteria
- Create/update rounds.
- Configure submission window.
- Create criteria templates and criteria items.

## Track & Topic
- Create tracks in event.
- Create topics in track.
- Record offline draw result by assigning topic to registered team.

## Team Management
- Create team.
- Invite/accept/reject member.
- Manage team leader/member while editable.

## Registration
- Team leader registers team for event/topic.
- Staff approves/rejects team.
- Staff bans/unbans team with RejectionReason.

## Round Participation
- Use `RoundDetails` to place registered teams into rounds.
- Track advancement/stopped teams via service/status rules.
- Admin can manually advance team to next round (`assign-next-round`).
- Admin can revert team to previous round (`revert-previous-round`), soft-deletes current round detail.
- Any authenticated user can check team's current round (`GET /api/v1/register-teams/{registerTeamId}/current-round`).

## Submission
- Team leader submits work for round.
- Support multiple submissions by multiple records.
- Judge/staff use latest valid submission before deadline.

## Judging & Scoring
- Judge views assigned track submissions.
- Judge scores by criteria item.
- Store totals in `Scores`, details in `ScoreItems`.
- Scoring helpers:
  - `RoundScoreHelper` — calculates **scopeScore**: AVG(judgeScore) per CriteriaItemId → SUM criteriaAvg → `Scores.TotalScore`.
  - `EventScoreHelper` — calculates **eventScore**: weighted average of round scores (weight_i = 1 default, denominator = total rounds in event, team ko tham gia round = 0).
  - `ChapterScoreHelper` — calculates **chapterScore**: AVG of event scores (chuẩn hóa giữa event có số round khác nhau).

## Report & Regrade
- User sends report.
- Team leader sends regrade request.
- Staff handles report/regrade manually.

## Leaderboard
- Event leaderboard and details by team.
- Year leaderboard by aggregating event leaderboard scores.

## Notification
- General notifications via `Notifications`.
- Mentor notices via `MentorNotifications`.

## Admin/System
- User management and global role updates via `Users.Role`.
- Assignment management.
- Event lifecycle control.
- Operational reports and manual decisions.
