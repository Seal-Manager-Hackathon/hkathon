# PATCH /api/v1/student/teams/{teamId}

> Student chỉnh sửa thông tin cơ bản của team. Chỉ leader mới được update.

**Controller:** [StudentTeamController.cs](Controllers/Student/StudentTeamController.cs)

## Nghiệp vụ
- Chỉ leader của team mới được update thông tin team.
- Hiện tại chỉ hỗ trợ đổi tên team.
- Nếu team bị disable → 404 Not Found.
- Nếu user ko phải leader → 400 Bad Request.

## Phân quyền
- ✅ Student (phải là leader của team)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team cần sửa |

### Body
```json
{
  "name": "Tên mới"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| name | ❌ | Tối đa 200 ký tự. Ko truyền = ko đổi |

## Response (200)
```json
{
  "data": null,
  "message": "Team Updated Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Only Team Leader Can Update Team | User ko phải leader | Ẩn nút sửa |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Ko phải Student | Ẩn chức năng |
| 404 | Team Not Found | teamId ko tồn tại hoặc đã bị disable | Báo "Team không tồn tại" |
