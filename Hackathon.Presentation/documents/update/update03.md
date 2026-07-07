# Update 03 — Chuẩn hóa Document Naming Convention & Fix nội dung doc

> Đồng bộ tên file doc với API, thêm doc còn thiếu, sửa nội dung sai.

## 1. Document Naming Convention

| Loại | Format | Ví dụ |
|------|--------|-------|
| List | `{role}.{entity}.list.md` | `admin.tracks.list.md` |
| Create | `{role}.{entity}.create.md` | `admin.tracks.create.md` |
| Detail | `{role}.{entity}.detail.md` | `admin.tracks.detail.md` |
| Update | `{role}.{entity}.update.md` | `admin.tracks.update.md` |
| Delete | `{role}.{entity}.delete.md` | `admin.tracks.delete.md` |
| Restore | `{role}.{entity}.restore.md` | `admin.tracks.restore.md` |
| Action | `{role}.{entity}.{action}.md` | `admin.register-teams.approve.md` |
| Sub list | `{role}.{parent}.{child}.list.md` | `admin.criteria-items.list.md` |

## 2. Rename files (24 files)

### Patch (thiếu action word)
- `user/patch/admin.users.userId.md` → `admin.users.update.md`
- `event/patch/admin.events.eventId.md` → `admin.events.update.md`
- `round/patch/admin.rounds.roundId.md` → `admin.rounds.update.md`

### Post (thiếu action word)
- `team/post/admin.teams.teamId.md` → `admin.teams.delete.md`
- `team/post/admin.teams.teamId.restore.md` → `admin.teams.restore.md`
- `notification/post/admin.notifications.notificationId.md` → `admin.notifications.delete.md`
- `notification/post/admin.notifications.notificationId.restore.md` → `admin.notifications.restore.md`
- `round/post/admin.events.rounds.md` → `admin.rounds.create.md`
- `round/post/admin.events.rounds.swap.md` → `admin.rounds.swap.md`

### Get (thiếu action word)
- `user/get/admin.users.userId.md` → `admin.users.detail.md`
- `event/get/admin.events.eventId.md` → `admin.events.detail.md`
- `team/get/admin.teams.teamId.md` → `admin.teams.detail.md`
- `register-team/get/admin.register-teams.registerTeamId.md` → `admin.register-teams.detail.md`
- `notification/get/admin.notifications.notificationId.md` → `admin.notifications.detail.md`
- `round/get/admin.events.rounds.md` → `admin.rounds.list.md`
- `round/get/admin.events.rounds.max-round-no.md` → `admin.rounds.max-round-no.md`

### Sub-resource (bỏ entityId dư)
- `register-team/post/admin.register-teams.registerTeamId.approve.md` → `admin.register-teams.approve.md`
- `register-team/post/admin.register-teams.registerTeamId.reject.md` → `admin.register-teams.reject.md`
- `register-team/post/admin.register-teams.registerTeamId.ban.md` → `admin.register-teams.ban.md`
- `register-team/post/admin.register-teams.registerTeamId.unban.md` → `admin.register-teams.unban.md`
- `register-team/get/admin.events.eventId.register-teams.md` → `admin.events.register-teams.md`
- `register-team/get/admin.users.userId.events.md` → `admin.users.events.md`
- `register-team/get/admin.teams.teamId.register-teams.md` → `admin.teams.register-teams.md`
- `user/get/admin.users.userId.teams.md` → `admin.users.teams.md`

## 3. Doc mới
- `team/patch/admin.teams.update.md` — PATCH /admin/teams/{teamId}
- `register-team/patch/admin.register-teams.update.md` — PATCH /admin/register-teams/{registerTeamId}
- `notification/patch/admin.notifications.update.md` — PATCH /admin/notifications/{notificationId}

## 4. Fix nội dung sai

| File | Nội dung cũ (sai) | Sửa thành |
|------|-------------------|-----------|
| `team/post/admin.teams.delete.md` | "đã disable → báo lỗi 400" | Admin soft delete không check |
| `team/post/admin.teams.restore.md` | "chưa disable → báo lỗi" | Admin restore không check |
| `register-team/post/admin.register-teams.ban.md` | "đã ban → báo lỗi 400" | Admin ban không check |
| `register-team/post/admin.register-teams.unban.md` | "chưa ban → báo lỗi" | Admin unban không check |
| `user/patch/admin.users.update.md` | Ví dụ ghi `PUT` | `PATCH` |
| `event/patch/admin.events.update.md` | Title ghi `PUT` | `PATCH` |
| `notification/post/admin.notifications.md` | Team: "gửi cho tất cả thành viên" | "Chỉ tạo 1 thông báo cho team" |
| `notification/post/admin.notifications.delete.md` | "disable → báo lỗi 400" | Admin soft delete không check |
| `notification/post/admin.notifications.restore.md` | "chưa disable → báo lỗi" | Admin restore không check |

## 5. Skill
- Cập nhật `.claude/skills/api-convention/SKILL.md` — thêm Document Naming Convention + template nội dung
