# GET /api/v1/mentor/tracks/{trackId}/mentor-notifications

> Mentor lấy danh sách các thông báo đã gửi cho 1 track (có phân trang).
> **Controller:** `MentorNotificationController` — `GET /api/v1/mentor/tracks/{trackId}/mentor-notifications`

## Nghiệp vụ

- Mentor xem lại lịch sử thông báo đã gửi cho 1 track.
- **Phải check mentor có assign track đó không.**
- Phân trang, sắp xếp mới nhất trước.

## Phân quyền
- ✅ Mentor — phải được assign vào track

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| trackId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 |

### Query Parameters
| Parameter | Type | Bắt buộc | Mặc định |
|-----------|------|----------|---------|
| pageIndex | int | Không | 1 |
| pageSize | int | Không | 10 |

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "Thông báo lịch thi",
        "description": "Vòng 1 sẽ diễn ra vào 10/07/2026",
        "assignTrackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "createdAt": "2026-07-10T12:00:00Z"
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
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
| 403 | You Are Not Assigned... | Mentor ko có quyền |
| 404 | Track Not Found | trackId ko tồn tại |

> **Ref:** API gốc cho Mentor.
