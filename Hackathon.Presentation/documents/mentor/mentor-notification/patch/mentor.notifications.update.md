# PATCH /api/v1/mentor/mentor-notifications/{mentorNotificationId}

> Mentor sửa title/description của 1 mentor notification.
> **Controller:** `MentorNotificationController` — `PATCH /api/v1/mentor/mentor-notifications/{mentorNotificationId}`

## Nghiệp vụ

- Mentor muốn sửa nội dung (title, description) của 1 notification đã gửi.
- **KHÔNG bắt buộc** phải truyền cả 2 field — chỉ truyền field nào cần sửa.
- Nếu notification không tồn tại → 404.

## Phân quyền
- ✅ Mentor (RoleEnum = Lecturer)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| mentorNotificationId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 |

### Body
```json
{
  "title": "Tiêu đề mới",
  "description": "Nội dung mới"
}
```

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
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Mentor Notification Not Found | mentorNotificationId ko tồn tại |

> **Ref:** API gốc cho Mentor.
