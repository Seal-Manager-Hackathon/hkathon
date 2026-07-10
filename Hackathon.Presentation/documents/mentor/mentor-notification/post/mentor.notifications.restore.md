# POST /api/v1/mentor/mentor-notifications/{mentorNotificationId}/restore

> Mentor khôi phục 1 mentor notification đã xóa mềm.
> **Controller:** `MentorNotificationController` — `POST /api/v1/mentor/mentor-notifications/{mentorNotificationId}/restore`

## Nghiệp vụ

- Mentor muốn khôi phục 1 notification đã bị xóa mềm trước đó.
- Set `IsDisable = false` — notification sẽ hiện lại trong danh sách.
- Nếu notification chưa bị xóa (IsDisable = false) → 400.

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
| 400 | Mentor Notification Is Already Active | Chưa bị xóa |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Mentor Notification Not Found | mentorNotificationId ko tồn tại |

> **Ref:** API gốc cho Mentor.
