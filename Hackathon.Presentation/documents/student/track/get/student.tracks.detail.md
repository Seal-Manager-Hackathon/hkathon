# GET /api/v1/student/tracks/{trackId}

> Student xem chi tiết 1 track.

**Controller:** [StudentTrackController.cs](Controllers/Student/StudentTrackController.cs)

## Nghiệp vụ
- Xem chi tiết track: title, maxTeam, số đội đã đăng ký.
- Nếu track bị disable (`IsDisable = true`) → 404.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| trackId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "title": "AI",
    "description": "...",
    "maxTeam": 20,
    "isDisable": false,
    "registerTeamCount": 15,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |
| 404 | Resource Not Found | trackId ko tồn tại / bị disable | Báo "Không tìm thấy track" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId})
