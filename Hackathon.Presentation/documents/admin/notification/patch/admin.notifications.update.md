# PATCH /api/v1/admin/notifications/{notificationId}

> Admin sửa thông báo.

## Nghiệp vụ
- Chỉ sửa được `title` và `description`
- Các field khác (TargetType, TeamId, UserId...) không thể sửa

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| notificationId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

Body:
```json
{
  "title": "Cập nhật tiêu đề",
  "description": "Nội dung mới"
}
```

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Notification Not Found | notificationId không tồn tại | Báo "Không tìm thấy thông báo" |
