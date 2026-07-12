# GET /api/v1/student/events/{eventId}/register-teams

> Student lấy danh sách register team đã Approved của 1 event.

**Controller:** [StudentRegisterTeamController.cs](Controllers/Student/StudentRegisterTeamController.cs)

## Nghiệp vụ
- Lấy danh sách register team đã được duyệt (status = Approved).
- **KHÔNG lấy** các team bị disable, bị banned, pending, rejected.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `...` |
| Keyword | string | ❌ | `Fteam` |
| RoundId | guid | ❌ | `...` |
| TrackId | guid | ❌ | `...` |
| TopicId | guid | ❌ | `...` |
| PageIndex | int | ❌ | `1` |
| PageSize | int | ❌ | `10` |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401/403 | Unauthorized/Forbidden | |
| 404 | Not Found | |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/register-teams)
