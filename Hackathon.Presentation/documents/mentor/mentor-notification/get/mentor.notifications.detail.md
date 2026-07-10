# GET /api/v1/mentor/mentor-notifications/{notificationId}

> Mentor xem chi tiết 1 mentor notification.
> **Controller:** `MentorNotificationController` — `GET /api/v1/mentor/mentor-notifications/{notificationId}`

## Nghiệp vụ

- Mentor xem chi tiết nội dung 1 thông báo đã gửi.

## Phân quyền
- ✅ Mentor

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| notificationId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 |

## Response (200)
```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "assignTrackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Thông báo lịch thi",
    "description": "Vòng 1 sẽ diễn ra vào 10/07/2026",
    "createdAt": "2026-07-10T12:00:00Z",
    "updatedAt": "2026-07-10T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Notification Not Found | notificationId ko tồn tại |

> **Ref:** API gốc cho Mentor.
