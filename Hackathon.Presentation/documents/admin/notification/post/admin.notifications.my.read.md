# POST /api/v1/admin/notifications/my/{notificationId}/read

> Admin đánh dấu 1 notification cụ thể là đã đọc.

## Nghiệp vụ

- Chỉ đọc được notification mà admin có quyền xem (Personal của họ hoặc System).
- Nếu notification đã `Read` rồi → không thay đổi gì.
- Access check: System → ai cũng đọc được, Personal → chỉ chủ sở hữu.

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| notificationId | Guid | ID của notification cần đánh dấu đã đọc |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không có quyền đọc notification này |
| 404 | Notification Not Found | ID không tồn tại |
