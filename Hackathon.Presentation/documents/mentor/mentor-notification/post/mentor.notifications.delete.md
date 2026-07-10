# POST /api/v1/mentor/mentor-notifications/{mentorNotificationId}/delete

> Mentor xóa mềm 1 mentor notification.
> **Controller:** `MentorNotificationController` — `POST /api/v1/mentor/mentor-notifications/{mentorNotificationId}/delete`

## Nghiệp vụ

- Mentor muốn xóa (soft-delete) 1 notification đã gửi.
- Set `IsDisable = true` — notification sẽ ko hiện trong danh sách nữa.
- Có thể khôi phục bằng API restore.
- Nếu notification đã disabled trước đó → 400.

## Phân quyền
- ✅ Mentor (RoleEnum = Lecturer)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| mentorNotificationId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 |

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
| 400 | Mentor Notification Is Already Disabled | Đã xóa trước đó |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Mentor Notification Not Found | mentorNotificationId ko tồn tại |

> **Ref:** API gốc cho Mentor.
