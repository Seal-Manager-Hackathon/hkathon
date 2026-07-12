# GET /api/v1/student/events/{eventId}

> Student xem chi tiết event.

**Controller:** [StudentEventController.cs](Controllers/Student/StudentEventController.cs)

## Nghiệp vụ
- Student xem chi tiết 1 event.
- Nếu event bị disable (`IsDisable = true`) hoặc Draft (`Status = Draft`) → 404.
- Giống Admin `GET /api/v1/admin/events/{eventId}` — response data như nhau.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "name": "Hackathon 2026",
    "description": "...",
    "startTime": "2026-08-01T00:00:00Z",
    "endTime": "2026-08-10T00:00:00Z",
    "registerLimitTime": "2026-07-25T00:00:00Z",
    "limitTeam": 50,
    "minMember": 3,
    "maxMember": 5,
    "status": "Published",
    "isDisable": false,
    "numberRound": 0,
    "season": "Summer",
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
| 404 | Event Not Found | eventId ko tồn tại / bị disable / Draft | Báo "Không tìm thấy sự kiện" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId})
