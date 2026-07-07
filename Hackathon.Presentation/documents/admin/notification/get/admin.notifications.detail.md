# GET /api/v1/admin/notifications/{notificationId}

> Lấy chi tiết toàn bộ thông tin của 1 thông báo.

## Nghiệp vụ
- Trả về tất cả fields của notification (kể cả UpdatedAt)
- 404 nếu notificationId không tồn tại

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| notificationId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "guid",
    "teamId": null,
    "title": "Thông báo mới",
    "status": "Unread",
    "description": "Nội dung chi tiết thông báo...",
    "targetType": "System",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:30:00Z"
  },
  "message": "Notification Detail Fetched Successfully",
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
