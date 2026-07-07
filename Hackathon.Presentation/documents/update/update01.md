# Update 01 — Chuẩn hóa HTTP Methods cho Admin APIs

## Quy tắc
| Chức năng | Method |
|-----------|--------|
| Lấy dữ liệu | GET |
| Tạo mới | POST |
| Xóa mềm | POST .../delete |
| Khôi phục | POST .../restore |
| Cập nhật thông tin | PATCH |
| Các action khác (approve, reject, ban, unban, swap) | POST |

> Không dùng PUT — tất cả update chuyển sang PATCH.
> Không dùng HttpPatch cho delete/restore — chuyển sang POST.

## Danh sách thay đổi

### 1. AdminUserController

| Endpoint | Method cũ | Method mới |
|----------|-----------|------------|
| `PUT /api/v1/admin/users/{userId}` | PUT | **PATCH** |

### 2. AdminEventController

| Endpoint | Method cũ | Method mới |
|----------|-----------|------------|
| `PUT /api/v1/admin/events/{eventId}` | PUT | **PATCH** |

### 3. AdminRoundController

| Endpoint | Method cũ | Method mới |
|----------|-----------|------------|
| `PUT /api/v1/admin/rounds/{roundId}` | PUT | **PATCH** |
| `PATCH /api/v1/admin/events/{eventId}/rounds/{roundId}/swap` | PATCH | **POST** |

### 4. AdminTrackController

| Endpoint | Method cũ | Method mới |
|----------|-----------|------------|
| `PUT /api/v1/admin/events/{eventId}/tracks/{trackId}` | PUT | **PATCH** |
| `PATCH /api/v1/admin/tracks/{trackId}/delete` | PATCH | **POST** |
| `PATCH /api/v1/admin/tracks/{trackId}/restore` | PATCH | **POST** |

### 5. AdminTopicController

| Endpoint | Method cũ | Method mới |
|----------|-----------|------------|
| `PUT /api/v1/admin/topics/{topicId}` | PUT | **PATCH** |

### 6. AdminTeamController

| Endpoint | Method cũ | Method mới |
|----------|-----------|------------|
| `PUT /api/v1/admin/teams/{teamId}` | PUT | **PATCH** |
| `PATCH /api/v1/admin/teams/{teamId}/delete` | PATCH | **POST** |
| `PATCH /api/v1/admin/teams/{teamId}/restore` | PATCH | **POST** |

### 7. AdminRegisterTeamController

| Endpoint | Method cũ | Method mới |
|----------|-----------|------------|
| `PUT /api/v1/admin/register-teams/{registerTeamId}` | PUT | **PATCH** |
| `PATCH /api/v1/admin/register-teams/{registerTeamId}/approve` | PATCH | **POST** |
| `PATCH /api/v1/admin/register-teams/{registerTeamId}/reject` | PATCH | **POST** |
| `PATCH /api/v1/admin/register-teams/{registerTeamId}/ban` | PATCH | **POST** |
| `PATCH /api/v1/admin/register-teams/{registerTeamId}/unban` | PATCH | **POST** |

### 8. AdminNotificationController

| Endpoint | Method cũ | Method mới |
|----------|-----------|------------|
| `PUT /api/v1/admin/notifications/{notificationId}` | PUT | **PATCH** |
| `PATCH /api/v1/admin/notifications/{notificationId}/delete` | PATCH | **POST** |
| `PATCH /api/v1/admin/notifications/{notificationId}/restore` | PATCH | **POST** |

### 9. AuthController & UserController

Không thay đổi — đã đúng convention (POST cho actions, GET cho retrieval).
