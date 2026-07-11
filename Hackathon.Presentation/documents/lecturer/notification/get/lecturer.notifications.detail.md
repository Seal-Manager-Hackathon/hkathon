# GET /api/v1/lecturer/notifications/{notificationId}

> Lecturer xem chi tiết thông báo.

**Controller:** [LecturerNotificationController.cs](Controllers/Lecturer/LecturerNotificationController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/notifications/{notificationId}`

- Giống hệt Admin `GET /api/v1/admin/notifications/{notificationId}`, khác auth là Lecturer.
- Trả về tất cả fields của notification (kể cả UpdatedAt).
- 404 nếu notificationId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| notificationId | Guid | ID của notification |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "userId": null,
    "teamId": null,
    "title": "Thông báo mới",
    "status": "Unread",
    "description": "Nội dung chi tiết thông báo...",
    "targetType": "System",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:30:00Z"
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Notification Not Found | notificationId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/notifications/{notificationId})
