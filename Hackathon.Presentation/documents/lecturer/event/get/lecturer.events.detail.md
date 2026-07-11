# GET /api/v1/lecturer/events/{eventId}

> Lecturer xem chi tiết thông tin event — gồm status, isDisable, season và tất cả các field khác.

**Controller:** [LecturerEventController.cs](Controllers/Lecturer/LecturerEventController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}`

- Giống hệt Admin `GET /api/v1/admin/events/{eventId}`, khác auth là Lecturer.
- Trả về tất cả fields của event (kể cả Status, IsDisable, Season, NumberRound, RegisterLimitTime...).
- 404 nếu eventId không tồn tại.
- Không check assign — Lecturer thấy bất kỳ event nào.

## Phân quyền
- ✅ Lecturer

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
    "numberRound": 3,
    "season": "Summer",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Event Not Found | eventId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId})
