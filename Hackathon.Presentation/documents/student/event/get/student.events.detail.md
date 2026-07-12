# GET /api/v1/student/events/{eventId}

> Student xem chi tiết event.

**Controller:** [StudentEventController.cs](Controllers/Student/StudentEventController.cs)

## Nghiệp vụ
- Student xem chi tiết 1 event.
- Nếu event bị disable (`IsDisable = true`) hoặc Draft → 404.
- Giống Admin `GET /api/v1/admin/events/{eventId}` — response như nhau.

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
    "description": "Event description",
    "startTime": "2026-07-01T00:00:00Z",
    "endTime": "2026-07-10T00:00:00Z",
    "registerLimitTime": "2026-07-05T00:00:00Z",
    "limitTeam": 50,
    "minMember": 2,
    "maxMember": 5,
    "status": "Published",
    "isDisable": false,
    "numberRound": 3,
    "season": "Summer",
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-01T00:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |
| 404 | Event Not Found | eventId ko tồn tại / bị disable / Draft |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId})
