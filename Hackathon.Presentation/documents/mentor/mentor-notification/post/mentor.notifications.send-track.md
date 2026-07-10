# POST /api/v1/mentor/tracks/{trackId}/mentor-notifications

> Mentor gửi thông báo tới 1 track mà họ được phân công.
> **Controller:** `MentorNotificationController` — `POST /api/v1/mentor/tracks/{trackId}/mentor-notifications`

## Nghiệp vụ

- Mentor muốn gửi 1 thông báo tới 1 track họ quản lý.
- **Phải check mentor có assign track này không**, nếu ko có quyền → 403.
- Thông báo lưu vào bảng `MentorNotifications`.

## Phân quyền
- ✅ Mentor — phải được assign vào track đó

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| trackId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 |

### Body
```json
{
  "title": "Thông báo lịch thi",
  "description": "Vòng 1 sẽ diễn ra vào 10/07/2026"
}
```

## Response (200)
```json
{
  "data": {
    "notificationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Thông báo lịch thi",
    "description": "Vòng 1 sẽ diễn ra vào 10/07/2026",
    "assignTrackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "createdAt": "2026-07-10T12:00:00Z"
  },
  "message": "Created Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Title/Description Is Required | Thiếu field |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned... | Mentor ko có quyền |
| 404 | Track Not Found | trackId ko tồn tại |

> **Ref:** API gốc cho Mentor.
