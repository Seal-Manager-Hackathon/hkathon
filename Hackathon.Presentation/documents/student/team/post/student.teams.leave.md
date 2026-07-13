# POST /api/v1/student/teams/{teamId}/leave

> Student rời khỏi team (chỉ member thường, leader không được rời).

## Nghiệp vụ

Thành viên rời khỏi team:
- Chỉ member thường mới được rời (không phải leader).
- **Nếu team bị khóa (CanEdit = false) → 400 Bad Request "Team Cannot Be Edited".**
- Nếu là leader: phải chuyển leader cho người khác trước hoặc giải thể team.
- Team phải tồn tại (không bị disable).
- Khi rời: set IsDisable = true, Status = Inactive.

## Phân quyền
- ✅ Student (RoleEnum = Student), phải là member của team.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 400 | Team Cannot Be Edited | Team bị khóa (CanEdit = false) |
| 400 | Team Leader Cannot Leave the Team. Please Change Leader First or Disband the Team. | Leader cố gắng rời team |
| 400 | You Are Already Inactive or Disabled in This Team | User đã inactive/disable rồi |
| 404 | Team Not Found | teamId không tồn tại hoặc đã disable |
| 404 | You Are Not a Member of This Team | User không phải member |
