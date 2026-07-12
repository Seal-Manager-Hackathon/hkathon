# GET /api/v1/student/teams/{teamId}/register-teams

> Student lấy danh sách register team Approved của 1 team.

**Controller:** [StudentRegisterTeamController.cs](Controllers/Student/StudentRegisterTeamController.cs)

## Nghiệp vụ
- Lấy danh sách register team của team, chỉ Approved, ko disable.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc |
|-------|------|----------|
| teamId | guid | ✅ (route) |

## Lỗi
| Status | message |
|--------|---------|
| 401/403 | Unauthorized/Forbidden |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/{teamId}/register-teams)
