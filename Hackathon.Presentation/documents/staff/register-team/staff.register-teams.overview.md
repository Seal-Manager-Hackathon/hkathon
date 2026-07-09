# Staff Register Team APIs — Overview

## Vai trò

Staff có thể quản lý register teams (đội đăng ký tham gia) trong các event mà họ được phân công. Các thao tác: duyệt, từ chối, ban/unban, assign track/topic, chuyển round.

## Nguyên tắc

- **Phải được phân công vào event** — mọi API mutation đều check `AssignEvents` trước
- **Filter IsDisable** — chỉ tác động lên register team không bị disable
- **Response giống Admin** — dùng chung DTO `RegisterTeamCard`, `RegisterTeamDetailResponse`, `AssignToNextRoundResponse`

## Danh sách APIs

| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `events/{eventId}/register-teams` | [Lấy danh sách teams của event](get-register-teams.md) |
| GET | `register-teams/{registerTeamId}` | [Xem chi tiết 1 team](get-register-team-detail.md) |
| PATCH | `register-teams/{registerTeamId}` | [Cập nhật team](update-register-team.md) |
| POST | `register-teams/{registerTeamId}/approve` | [Duyệt team](approve-register-team.md) |
| POST | `register-teams/{registerTeamId}/reject` | [Từ chối team](reject-register-team.md) |
| POST | `register-teams/{registerTeamId}/ban` | [Cấm team](ban-register-team.md) |
| POST | `register-teams/{registerTeamId}/unban` | [Bỏ cấm team](unban-register-team.md) |
| POST | `register-teams/{registerTeamId}/assign-next-round` | [Chuyển vòng](assign-next-round.md) |
| POST | `register-teams/{registerTeamId}/revert-previous-round` | [Quay lại vòng trước](revert-previous-round.md) |
| POST | `register-teams/{registerTeamId}/assign-track-topic` | [Gán track/topic](assign-track-topic.md) |
| POST | `register-teams/{registerTeamId}/remove-track-topic` | [Xóa track/topic](remove-track-topic.md) |
| GET | `teams/{teamId}/register-teams` | [DS team đăng ký của 1 team](get-register-teams-by-team.md) |
| GET | `users/{userId}/events` | [DS event của 1 user](get-user-events.md) |