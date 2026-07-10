# GET /api/v1/mentor/events/{eventId}/tracks

> Mentor lấy danh sách các track được phân công trong 1 event.
> **Controller:** `MentorNotificationController` — `GET /api/v1/mentor/events/{eventId}/tracks`

## Nghiệp vụ

- Mentor muốn xem các track nào họ được phân công (assign) trong 1 event cụ thể.
- **Chỉ lấy track có IsDisable = false** và **track đó phải được assign cho mentor** (qua bảng AssignTracks).
- Mentor phải có AssignEvent trong event đó trước.
- KHÔNG phân trang — trả về toàn bộ danh sách.

## Phân quyền
- ✅ Mentor (RoleEnum = Lecturer, có AssignEvent trong event)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| eventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 |

## Response (200)
```json
{
  "data": [
    {
      "assignTrackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "trackTitle": "Tri tue nhan tao",
      "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "eventName": "Hackathon 2026"
    }
  ],
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Event Not Found or You Are Not Assigned to This Event | eventId ko tồn tại hoặc mentor chưa được assign |

> **Ref:** API gốc cho Mentor.
