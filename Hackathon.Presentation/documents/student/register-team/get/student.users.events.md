# GET /api/v1/student/users/{userId}/events

> Student lấy danh sách event mà user đã đăng ký team (Approved).

**Controller:** [StudentRegisterTeamController.cs](Controllers/Student/StudentRegisterTeamController.cs)

## Nghiệp vụ
- Lấy danh sách event mà user đã đăng ký, Approved.
- Tự động lấy user từ token nếu userId ko truyền.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc |
|-------|------|----------|
| userId | guid | ✅ (route) |

## Lỗi
| Status | message |
|--------|---------|
| 401/403 | Unauthorized/Forbidden |

> **Ref:** [Admin API tương ứng](/api/v1/admin/users/{userId}/events)
