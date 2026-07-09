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
| GET | `events/{eventId}/register-teams` | [Lấy danh sách teams của event](get/staff.events.register-teams.md) |
| GET | `register-teams/{registerTeamId}` | [Xem chi tiết 1 team](get/staff.register-teams.detail.md) |
| PATCH | `register-teams/{registerTeamId}` | [Cập nhật team](patch/staff.register-teams.update.md) |
| POST | `register-teams/{registerTeamId}/approve` | [Duyệt team](post/staff.register-teams.approve.md) |
| POST | `register-teams/{registerTeamId}/reject` | [Từ chối team](post/staff.register-teams.reject.md) |
| POST | `register-teams/{registerTeamId}/ban` | [Cấm team](post/staff.register-teams.ban.md) |
| POST | `register-teams/{registerTeamId}/unban` | [Bỏ cấm team](post/staff.register-teams.unban.md) |
| POST | `register-teams/{registerTeamId}/assign-next-round` | [Chuyển vòng](post/staff.register-teams.assign-next-round.md) |
| POST | `register-teams/{registerTeamId}/revert-previous-round` | [Quay lại vòng trước](post/staff.register-teams.revert-previous-round.md) |
| POST | `register-teams/{registerTeamId}/assign-track-topic` | [Gán track/topic](post/staff.register-teams.assign-track-topic.md) |
| POST | `register-teams/{registerTeamId}/remove-track-topic` | [Xóa track/topic](post/staff.register-teams.remove-track-topic.md) |
| GET | `teams/{teamId}/register-teams` | [DS team đăng ký của 1 team](get/staff.teams.register-teams.md) |
| GET | `users/{userId}/events` | [DS event của 1 user](get/staff.users.events.md) |
