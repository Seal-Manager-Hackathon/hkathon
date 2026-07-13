# POST /api/v1/student/teams/{teamId}/disband

> Student giải thể team. Chỉ leader mới được giải thể.

**Controller:** [StudentTeamController.cs](Controllers/Student/StudentTeamController.cs)

## Nghiệp vụ
- Chỉ leader của team mới được disband.
- **Nếu team bị khóa (CanEdit = false) → 400 Bad Request "Team Cannot Be Edited".**
- Khi disband:
  - Tất cả member trong team bị set IsDisable = true và Status = Inactive.
- Team bị set IsDisable = true và CanEdit = false.
- Nếu team đã bị disable → 404 Not Found.
- Nếu user ko phải leader → 400 Bad Request.

## Phân quyền
- ✅ Student (phải là leader của team)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team cần giải thể |

## Response (200)
```json
{
  "data": null,
  "message": "Deleted Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Team Cannot Be Edited | Team bị khóa (CanEdit = false) | Ẩn nút giải thể |
| 400 | Only Team Leader Can Disband Team | User ko phải leader | Ẩn nút giải thể |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Ko phải Student | Ẩn chức năng |
| 404 | Team Not Found | teamId ko tồn tại hoặc đã bị disable | Báo "Team không tồn tại" |
