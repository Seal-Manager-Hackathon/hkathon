# GET /api/v1/staff/teams/{teamId}

> Xem chi tiết 1 team — gồm thông tin cơ bản và danh sách thành viên.

## Nghiệp vụ
- Trả về thông tin team: tên, trạng thái chỉnh sửa, disable
- Kèm danh sách thành viên (userId, email, tên, avatar, leader, status)

## Phân quyền
- ✅ Staff

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| teamId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "name": "FTeam",
    "canEdit": true,
    "isDisable": false,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z",
    "members": [
      {
        "userId": "guid",
        "email": "user@example.com",
        "firstName": "Nguyễn",
        "lastName": "Văn A",
        "avatarUrl": "https://robohash.org/...",
        "isLeader": true,
        "status": "Active"
      }
    ]
  },
  "message": "Team Detail Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 404 | Team Not Found | teamId không tồn tại | Hiển thị thông báo |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff | Ẩn chức năng |