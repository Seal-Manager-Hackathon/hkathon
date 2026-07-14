# GET /api/v1/admin/notifications/my/{notificationId}

> Admin lấy chi tiết 1 notification của riêng họ (Personal + System).

## Nghiệp vụ

- Chỉ lấy được notification mà admin có quyền xem.
- Nếu notification là System → ai cũng xem được.
- Nếu notification là Personal → phải là chủ sở hữu.
- Team notifications → 403 Forbidden (admin ko xem được team notifications qua my).

## Phân quyền
- ✅ Admin

## Request

| Parameter | Type | Description |
|-----------|------|-------------|
| notificationId | Guid | ID của notification |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "userId": "guid",
    "teamId": null,
    "title": "Thông báo",
    "status": "Unread",
    "description": "Nội dung chi tiết...",
    "targetType": "Personal",
    "createdAt": "2026-07-14T12:00:00Z",
    "updatedAt": "2026-07-14T12:00:00Z"
  },
  "message": "Notification Detail Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Xem notification của người khác |
| 404 | Not Found | ID không tồn tại |
