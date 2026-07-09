# Base API — Tổng quan

## Vai trò

Base API dành cho tất cả người dùng đã đăng nhập, không phân biệt role.

## Nguyên tắc xác thực

- JWT token hợp lệ qua `Authorization: Bearer {token}`
- Không kiểm tra role cụ thể — chỉ cần đăng nhập

## Danh sách API

### User
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/user/me` | [Lấy thông tin profile](user/get/user.me.md) |
| PATCH | `/api/v1/user/me` | [Cập nhật profile](user/patch/user.me.md) |
| POST | `/api/v1/user/avatar` | [Upload avatar](user/post/user.avatar.md) |

### Register Team
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/register-teams/{registerTeamId}/current-round` | [Xem round hiện tại](register-team/get/register-teams.current-round.md) |

### Notification
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/notifications/{notificationId}` | [Xem chi tiết thông báo](notification/get/base.notifications.detail.md) |
