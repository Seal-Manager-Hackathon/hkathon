# GET /api/v1/notifications/{notificationId}

> Người dùng đã đăng nhập lấy thông tin chi tiết của 1 thông báo (Notification) kèm phân quyền an toàn.

## Nghiệp vụ

API này cho phép bất kỳ người dùng nào đã đăng nhập (Student, Lecturer, Staff, Admin) xem chi tiết thông báo, miễn là họ có quyền xem:
- **System notification:** Tất cả người dùng trong hệ thống đều xem được.
- **Personal notification:** Chỉ người dùng nhận (UserId trùng với Token UserId) mới xem được.
- **Team notification:** Chỉ thành viên trong Team nhận thông báo (TeamId trùng với 1 trong các team mà user tham gia) mới xem được.

Nếu không phải các trường hợp trên, trả về `403 Forbidden` (hoặc `404` nếu không tìm thấy/thông báo bị disable).

## Phân quyền
- ✅ Đăng nhập (Mọi role: Student, Lecturer, Staff, Admin)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| notificationId | Guid | ID của thông báo |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "userId": "guid/null",
    "teamId": "guid/null",
    "title": "Thông báo khẩn cấp",
    "status": "Active",
    "description": "Nội dung chi tiết thông báo...",
    "targetType": "System",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `id` | ID của thông báo |
| `userId` | User nhận (đối với Personal notification) |
| `teamId` | Team nhận (đối với Team notification) |
| `title` | Tiêu đề thông báo |
| `status` | Trạng thái (Active/Inactive) |
| `description` | Nội dung thông báo |
| `targetType` | Đối tượng đích: System, Personal, Team |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | You do not have permission to view this notification | Xem thông báo của người khác hoặc team khác |
| 404 | Notification Not Found | ID không tồn tại hoặc thông báo đã bị disable (IsDisable = true) |
