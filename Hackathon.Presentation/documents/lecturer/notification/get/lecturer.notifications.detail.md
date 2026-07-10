# GET /api/v1/lecturer/notifications/{notificationId}

> Lecturer xem chi tiết một thông báo.

## Nghiệp vụ
- Chỉ xem được thông báo System hoặc Personal dành cho mình.
- Nếu thông báo là Personal của người khác → 403.

## Phân quyền
- ✅ Lecturer

## Request
| Param | Kiểu | Bắt buộc | Ghi chú |
|-------|------|----------|---------|
| notificationId | Guid | ✅ | ID thông báo |

## Response (200)
```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": null,
    "teamId": null,
    "title": "Hackathon AI 2026 đã kết thúc",
    "status": "Unread",
    "description": "Cảm ơn bạn đã tham gia...",
    "targetType": "System",
    "createdAt": "2026-07-10T00:00:00Z",
    "updatedAt": "2026-07-10T00:00:00Z"
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-10T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Lecturer hoặc ko có quyền xem |
| 404 | Notification Not Found | ID không tồn tại |
