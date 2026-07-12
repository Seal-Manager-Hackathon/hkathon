# GET /api/v1/student/register-teams/{registerTeamId}

> Student xem chi tiết 1 register team (chỉ Approved).

**Controller:** [StudentRegisterTeamController.cs](Controllers/Student/StudentRegisterTeamController.cs)

## Nghiệp vụ
- Xem chi tiết register team: thông tin team, event, track, topic, members.
- **Chỉ xem được** register team Approved, ko disable, ko banned.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc |
|-------|------|----------|
| registerTeamId | guid | ✅ (route) |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401/403 | Unauthorized/Forbidden | |
| 404 | Register Team Not Found | ko tồn tại / disable / banned / ko Approved |

> **Ref:** [Admin API tương ứng](/api/v1/admin/register-teams/{registerTeamId})
