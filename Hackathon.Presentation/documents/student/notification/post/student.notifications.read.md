# POST /api/v1/student/notifications/{notificationId}/read

> Student đánh dấu 1 notification cụ thể là đã đọc.

## Nghiệp vụ

API này cho phép student đánh dấu 1 notification (Personal/Team/System) từ `Unread`/`Pending` thành `Read`.

- Chỉ đọc được notification mà student có quyền xem.
- Nếu notification đã `Read` rồi → không thay đổi gì (idempotent).
- Access control: System → tất cả, Personal → chủ sở hữu, Team → thành viên team.

## Phân quyền
- ✅ Student (đã đăng nhập, token hợp lệ)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| notificationId | Guid | ID của notification cần đánh dấu đã đọc |

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | You Do Not Have Access to This Notification | Không có quyền đọc notification này |
| 404 | Notification Not Found | ID không tồn tại hoặc đã bị disable |
