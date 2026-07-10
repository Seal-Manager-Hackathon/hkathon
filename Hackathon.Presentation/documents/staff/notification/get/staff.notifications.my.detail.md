# GET /api/v1/staff/notifications/my/{notificationId}

> Staff xem chi tiết thông báo.

## Nghiệp vụ
- Chỉ xem được System hoặc Personal của mình.

## Phân quyền
- ✅ Staff

## Request
| Param | Kiểu | Bắt buộc | Ghi chú |
|-------|------|----------|---------|
| notificationId | Guid | ✅ | ID thông báo |

## Response (200)
```json
{
  "data": {
    "id": "guid", "userId": null, "teamId": null,
    "title": "Thông báo", "status": "Unread",
    "description": "...", "targetType": "System",
    "createdAt": "...", "updatedAt": "..."
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-10T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không có quyền |
| 404 | Notification Not Found | ID không tồn tại |

> **First** — API này là bản gốc, không có Admin tương ứng. Dùng làm chuẩn tham chiếu cho các role khác.
