# GET /api/v1/lecturer/rounds/{roundId}

> Lecturer xem chi tiết 1 round — gồm tất cả thông tin event, thời gian, submission, limitTeam, isDisable.

**Controller:** [LecturerRoundController.cs](Controllers/Lecturer/LecturerRoundController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/rounds/{roundId}`

- Giống hệt Admin `GET /api/v1/admin/rounds/{roundId}`, khác auth là Lecturer.
- Trả về tất cả fields của round (kể cả EventName, IsDisable, LimitTeam...).
- 404 nếu roundId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| roundId | Guid | ID của round |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "name": "Vòng 1",
    "description": "Vòng loại",
    "roundNo": 1,
    "startTime": "2026-07-10T00:00:00Z",
    "endTime": "2026-07-15T00:00:00Z",
    "startSubmission": "2026-07-11T00:00:00Z",
    "endSubmission": "2026-07-14T00:00:00Z",
    "limitTeam": 50,
    "isDisable": false,
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-01T00:00:00Z"
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
| 404 | Resource Not Found | roundId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId})
